using CarBuilder.Models;
using CarBuilder.Models.DTOs;

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

List<Order> orders = new List<Order>
{
   new Order { Id = 1, Timestamp = DateTime.Now, WheelId = 1, TechnologyId = 2, PaintId = 3, InteriorId = 1}
};



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options =>
                {
                    options.AllowAnyOrigin();
                    options.AllowAnyMethod();
                    options.AllowAnyHeader();
                });
}

app.UseHttpsRedirection();

app.MapGet("/paintcolors", () => 
{
    return paintColors.Select(p => new PaintColorDTO
    {
        Id = p.Id,
        Price = p.Price,
        Color = p.Color
    });
});

app.MapGet("/interiors", () => 
{
    return interiors.Select(i => new InteriorDTO
    {
        Id = i.Id,
        Price = i.Price,
        Material = i.Material
    });
});

app.MapGet("/technologies", () => 
{
    return technologies.Select(t => new TechnologyDTO
    {
        Id = t.Id,
        Price = t.Price,
        Package = t.Package
    });
});

app.MapGet("/wheels", () => 
{
    return wheels.Select(w => new WheelsDTO 
    {
        Id = w.Id,
        Price = w.Price,
        Style = w.Style
    });
});

app.MapGet("/orders", () => 
{
    List<OrderDTO> orderDTOs = orders.Where(o => !o.Fulfilled).Select(o => 
    {
        Wheels wheel = wheels.FirstOrDefault(w => w.Id == o.WheelId);
        Technology technology = technologies.FirstOrDefault(t => t.Id == o.TechnologyId);
        PaintColor paint = paintColors.FirstOrDefault(p => p.Id == o.PaintId);
        Interior interior = interiors.FirstOrDefault(i => i.Id == o.InteriorId);
        
        return new OrderDTO
        {
            Id = o.Id,
            Timestamp = o.Timestamp,
            WheelId = o.WheelId,
            Wheels = new WheelsDTO
            {
                Id = wheel.Id,
                Price = wheel.Price,
                Style = wheel.Style
            },
            TechnologyId = o.TechnologyId,
            Technology = new TechnologyDTO
            {
                Id = technology.Id,
                Price = technology.Price,
                Package = technology.Package
            },
            PaintId = o.PaintId,
            Paint = new PaintColorDTO
            {
                Id = paint.Id,
                Price = paint.Price,
                Color = paint.Color
            },
            InteriorId = o.InteriorId,
            Interior = new InteriorDTO
            {
                Id = interior.Id,
                Price = interior.Price,
                Material = interior.Material
            },
            Fulfilled = o.Fulfilled
        };
    }).ToList();

    return orderDTOs;
});

app.MapPost("orders", (Order order) => 
{
    // Get all the data to check that the ids for the order are valid
    Wheels wheel = wheels.FirstOrDefault(w => w.Id == order.WheelId);
    Technology technology = technologies.FirstOrDefault(t => t.Id == order.TechnologyId);
    PaintColor paint = paintColors.FirstOrDefault(c => c.Id == order.PaintId);
    Interior interior = interiors.FirstOrDefault(i => i.Id == order.InteriorId);

    // if the client did not provide a valid, this is a bad request
    if (wheel == null)
    {
        return Results.BadRequest("Invalid Wheel ID provided.");
    }
    if (technology == null)
    {
        return Results.BadRequest("Invalid Technology ID provided.");
    }
    if (paint == null)
    {
        return Results.BadRequest("Invalid Paint ID provided.");
    }
    if (interior == null)
    {
        return Results.BadRequest("Invalid Interior ID provided.");
    }

    //Create a new Id for the order
    order.Id = orders.Max(o => o.Id) + 1;
    order.Timestamp = DateTime.Now;
    orders.Add(order);

    return Results.Created($"/orders/{order.Id}", new OrderDTO
    {
        Id = order.Id,
        Timestamp = order.Timestamp,
        WheelId = order.WheelId,
        Wheels = new WheelsDTO
        {
            Id = wheel.Id,
            Price = wheel.Price,
            Style = wheel.Style
        },
        TechnologyId = order.TechnologyId,
        Technology = new TechnologyDTO
        {
            Id = technology.Id,
            Price = technology.Price,
            Package = technology.Package
        },
        PaintId = order.PaintId,
        Paint = new PaintColorDTO
        {
            Id = paint.Id,
            Price = paint.Price,
            Color = paint.Color
        },
        InteriorId = order.InteriorId,
        Interior = new InteriorDTO
        {
            Id = interior.Id,
            Price = interior.Price,
            Material = interior.Material
        }
    });
});

app.MapPost("/orders/{orderId}/fulfill", (int orderId) =>
{
    Order orderToComplete = orders.FirstOrDefault(o => o.Id == orderId);

    if (orderToComplete  == null)
    {
        return Results.NotFound();
    }
    if (orderToComplete.Fulfilled == true)
    {
        return Results.BadRequest("Order is already fulfilled.");
    }

    orderToComplete.Fulfilled = true;

    return Results.NoContent();
});

app.Run();