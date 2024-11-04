using Microsoft.EntityFrameworkCore;
using System.Runtime;
using Microsoft.AspNetCore.Mvc;
using ufoShopBack.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.IdentityModel.Tokens.Jwt;
using System;
using ufoShopBack.Services;
using ufoShopBack.Data.Entities;
using ufoShopBack.CRUDoperations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = false,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddScoped<UsersCRUD>();
builder.Services.AddScoped<ProductCRUD>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ShowOrderService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.IgnoreNullValues = true;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient",
        policy => {
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<Context>();
}

app.UseCors("AngularClient");

app.MapGet("/user", async (UsersCRUD user) => Results.Ok(await user.GetAsync()))
    .Produces<List<User>>(StatusCodes.Status200OK);

app.MapGet("/user/{id}", async (UsersCRUD user, int id) => await user.GetAsync(id) is User user1
    ? Results.Ok(user1)
    : Results.NotFound())
    .Produces<List<User>>(StatusCodes.Status200OK);
app.MapGet("/user/email/{email}", async (UsersCRUD user, string email) => await user.GetByEmailAsync(email) is User user1
    ? Results.Ok(user1)
    : Results.NotFound())
    .Produces<List<User>>(StatusCodes.Status200OK);

app.MapPost("/user", async ([FromBody] User userBody, UsersCRUD user, HttpResponse response) =>
{
    await user.CreateAsync(userBody);
    return Results.Created($"/user/{userBody.Id}", userBody);
});

app.MapPut("/user/{id}", async ([FromBody] User userBody, UsersCRUD user, int id) =>
{
    await user.UpdateAsync(userBody, id);
    return Results.NoContent();
});

app.MapDelete("/user/{id}", async (UsersCRUD user, int id) =>
{
    await user.DeleteAsync(id);

});


app.MapGet("/product", async (ProductCRUD product) => await product.GetAsync())
    .Produces<List<Product>>(StatusCodes.Status200OK);
app.MapGet("/product/{id}", async (ProductCRUD product, int id) => await product.GetAsync(id) is Product product1
    ? Results.Ok(product)
    : Results.NotFound())
    .Produces(StatusCodes.Status200OK);

app.MapPost("/product", async ([FromBody]Product productBody, ProductCRUD product) =>
{
    await product.CreateAsync(productBody);
    return Results.Created();

});

app.MapPut("/product/{id}", async([FromBody] Product productBody, ProductCRUD product, int id) =>
{
    await product.UpdateAsync(productBody, id);
    return Results.NoContent();
});

app.MapDelete("/product/{id}", async (ProductCRUD product, int id) =>
{
    await product.DeleteAsync(id);
});

app.MapPost("/sign up", async ([FromBody] User loginData, UserService userRepo, UsersCRUD usersCRUD) =>
{
    var result = await userRepo.IsUserNameUniqueAsync(loginData.Nickname);
    if (!result)
    {
        return Results.BadRequest();
    }

    result = await userRepo.IsEmailUniqueAsync(loginData.Email);
    if (!result) { 
        return Results.BadRequest();
    }
    await usersCRUD.CreateAsync(loginData);
    return Results.Created();
});

app.MapPost("/login", async ([FromBody] User loginData, UserService userRepo, UsersCRUD usersCRUD) =>
{

    var userFromDb = await usersCRUD.GetByEmailAsync(loginData.Email);
    if (userFromDb == null || !userRepo.VerifyPassword(userFromDb.Password, loginData.Password))
    {
        return Results.NotFound();
    };

    if (userFromDb == null) return Results.Unauthorized();

    var claims = new List<Claim> { new Claim(ClaimTypes.Name, userFromDb.Email) };
    var jwt = new JwtSecurityToken(
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.ISSUER,
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromDays(2)),
        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );
    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
    Console.WriteLine($"Generated JWT: {encodedJwt}");

    var response = new
    {
        access_token = encodedJwt,
        username = userFromDb.Email
    };
    return Results.Json(response);
});

app.Map("/data", [Authorize] (HttpContext context) => new { message = "xdxd" });
app.MapGet("/orders/{id}", async (int id, UsersCRUD usersCRUD, ShowOrderService ordersData) =>
{
    var userFromDb = await usersCRUD.GetAsync(id);
    if (userFromDb == null)
    {
        return Results.NotFound();
    }
    var orders = await ordersData.ShowOrderAsync(userFromDb);
    return Results.Ok(orders);
});

app.MapControllers();
app.Run();



public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; 
    public const string AUDIENCE = "MyAuthClient"; 
    const string KEY = "AG8f2sI7gKf9HnPm0QdS9RkYxLtVpZoU"; 
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}

