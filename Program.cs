using System.Diagnostics;

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

var samples = new List<Sample>
{
    new Sample { Id = 1, Title = "Empty" },
    new Sample { Id = 2, Title = "Very Empty" },
    new Sample { Id = 2, Title = "Very Very Empty" },
    new Sample { Id = 2, Title = "Very Very Very Empty" },
};
app.UseHttpsRedirection();
app.MapGet("/sample", () => samples);
app.MapGet("/sample{id}", (int id) =>
{
    var sample = samples.Find(x => x.Id == id);
    return sample is null ? Results.NotFound("No samples on that id") : Results.Ok(sample);
});

app.MapPost("sample", (Sample sample) =>
{
    samples.Add(sample);
    return samples;
});
app.MapPut("/sample{id}", (Sample updatedSample, int id) =>
{
    var sample = samples.Find(x => x.Id == id);
    if (sample is not null)
    {
        sample.Title = updatedSample.Title;
        Results.Ok(samples);
    }

    Results.NotFound("No samples on that id");
});
app.MapDelete("/sample{id}", (int id) =>
{
    var sample = samples.Find(x => x.Id == id);
    if (sample is not null)
    {
        samples.Remove(sample);
        Results.Ok(samples);
    }

    Results.NotFound("No samples on that id");
});


app.Run();


class Sample
{
    public int Id { get; set; }
    public required string Title { get; set; }
}