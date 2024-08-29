using CarBuilder.Models;

List<PaintColor> paintColors = new List<PaintColor>
        {
            new PaintColor { Id = 1, Price = 200m, Color = "Silver" },
            new PaintColor { Id = 2, Price = 250m, Color = "Midnight Blue" },
            new PaintColor { Id = 3, Price = 300m, Color = "Firebrick Red" },
            new PaintColor { Id = 4, Price = 150m, Color = "Spring Green" }
        };

List<Interior> interiors = new List<Interior>
        {
            new Interior { Id = 1, Price = 500m, Material = "Beige Fabric" },
            new Interior { Id = 2, Price = 550m, Material = "Charcoal Fabric" },
            new Interior { Id = 3, Price = 800m, Material = "White Leather" },
            new Interior { Id = 4, Price = 850m, Material = "Black Leather" }
        };

List<Technology> technologies = new List<Technology>
        {
            new Technology { Id = 1, Price = 1000m, Package = "Basic Package (basic sound system)" },
            new Technology { Id = 2, Price = 1500m, Package = "Navigation Package (includes integrated navigation controls)" },
            new Technology { Id = 3, Price = 1200m, Package = "Visibility Package (includes side and rear cameras)" },
            new Technology { Id = 4, Price = 2000m, Package = "Ultra Package (includes navigation and visibility packages)" }
        };

List<Wheels> wheels = new List<Wheels>
        {
            new Wheels { Id = 1, Price = 400m, Style = "17-inch Pair Radial" },
            new Wheels { Id = 2, Price = 450m, Style = "17-inch Pair Radial Black" },
            new Wheels { Id = 3, Price = 500m, Style = "18-inch Pair Spoke Silver" },
            new Wheels { Id = 4, Price = 550m, Style = "18-inch Pair Spoke Black" }
        };

List<Order> orders = new List<Order>();


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Run();