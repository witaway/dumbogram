namespace Dumbogram;

internal class Program
{
    private static readonly bool IsDevelopment =
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

    public static int Main(string[] args)
    {
        // var a = new KeysetOrder<Message>()
        //     .Ascending(p => p.ChatId)
        //     .Ascending(p => p!.SenderProfile!.UpdatedDate)
        //     .Descending(p => p!.Chat!.OwnerProfile!.Description!);
        //
        // var e = new Message
        // {
        //     ChatId = Guid.NewGuid(),
        //     SenderProfile = new UserProfile
        //     {
        //         UpdatedDate = DateTimeOffset.Now
        //     },
        //     Chat = new Chat
        //     {
        //         OwnerProfile = new UserProfile
        //         {
        //             Description = "Hello!"
        //         }
        //     }
        // };
        //
        // var cursorEncoded = Cursor<Message>.Encode(a, e);
        // Console.WriteLine(cursorEncoded);
        // var cursorDecoded = Cursor<Message>.Decode(a, cursorEncoded);
        //
        // foreach (var b in cursorDecoded.Values)
        // {
        //     if (b is KeysetColumnValue<Message, Guid> abc1)
        //     {
        //         Console.WriteLine(abc1.Path);
        //         Console.WriteLine(abc1.Type);
        //         Console.WriteLine(abc1.Value);
        //     }
        //     else if (b is KeysetColumnValue<Message, string> abc2)
        //     {
        //         Console.WriteLine(abc2.Path);
        //         Console.WriteLine(abc2.Type);
        //         Console.WriteLine(abc2.Value);
        //     }
        //     else if (b is KeysetColumnValue<Message, DateTimeOffset> abc3)
        //     {
        //         Console.WriteLine(abc3.Path);
        //         Console.WriteLine(abc3.Type);
        //         Console.WriteLine(abc3.Value);
        //     }
        // }
        //
        // // a.Columns[0].PropertySelector.Body.NodeType == Co
        //
        //
        // return 0;

        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.RegisterApplicationServices(builder.Configuration);

            var app = builder.Build();

            app.ConfigureMiddleware();
            app.RegisterEndpoints();

            app.Run();
        }
        // Expose exceptions when environment is Development
        // That's because there are exception that throws only when development
        // Example is HostAbortedException, showing up when for example "dotnet ef migrations add"
        // Todo: Think about it. Maybe use: "when (exception is not HostAbortedException)" is better solution?
        catch (Exception exception) when (!IsDevelopment)
        {
            // TEMPORARY SOLUTION!!
            // Todo: Change WriteLine to normal logger
            // How to? Logger initializes after app.Build(),
            //    but errors might be occured later during WebApplicationBuilder configuration
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[FATAL] Error has occured during initialization");
            Console.ResetColor();
            Console.WriteLine(exception);
            return exception.HResult;
        }

        return 0;
    }
}