using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace ZondBeat_TGBot
{
    internal class Program
    {
        private static TelegramBotClient botClient;
        private static string connectionString = "Your_SQL_Server_Connection_String";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var botClient = new TelegramBotClient("5933916083:AAGOlYMlIokt96_3NEAqcNFDdpFRdsHPpuM");

            using CancellationTokenSource cts = new();

            ReceiverOptions receiverOptions = new() { AllowedUpdates = Array.Empty<UpdateType>() };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token);

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for {me.Username}");
            Console.ReadLine();

            cts.Cancel();
        }

        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId, text: "You said:\n" + messageText, cancellationToken: cancellationToken);


            if (message.Text == "Check")
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Check: OK",
                    cancellationToken: cancellationToken);
            }
        }

        static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestExeption
                        => $"Telegram API Error:\n[{apiRequestExeption.ErrorCode}]\n{apiRequestExeption.Message}" + exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}



//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using Telegram.Bot;
//using Telegram.Bot.Args;
//using Telegram.Bot.Types;
//using Telegram.Bot.Types.Enums;
//using Telegram.Bot.Types.ReplyMarkups;

//namespace MusicBot
//{
//    public class Program
//    {
//        private static TelegramBotClient botClient;
//        private static string connectionString = "your_connection_string"; // Replace with your SQL Server connection string

//        static void Main(string[] args)
//        {
//            botClient = new TelegramBotClient("your_bot_token"); // Replace with your Telegram Bot token

//            botClient.OnMessage += BotClient_OnMessage;
//            botClient.StartReceiving();

//            Console.WriteLine("Bot started. Press any key to exit.");
//            Console.ReadKey();

//            botClient.StopReceiving();
//        }

//        private static async void BotClient_OnMessage(object sender, MessageEventArgs e)
//        {
//            var message = e.Message;

//            if (message.Type == MessageType.Text)
//            {
//                switch (message.Text)
//                {
//                    case "/start":
//                        await SendCategories(message.Chat.Id);
//                        break;
//                    case "Hip-hop":
//                    case "Drill":
//                    case "Vibe":
//                    case "Detroit":
//                        await SendTracks(message.Chat.Id, message.Text);
//                        break;
//                    default:
//                        await botClient.SendTextMessageAsync(message.Chat.Id, "Invalid command.");
//                        break;
//                }
//            }
//        }

//        private static async Task SendCategories(long chatId)
//        {
//            var categories = new List<string> { "Hip-hop", "Drill", "Vibe", "Detroit" };

//            var replyKeyboardMarkup = new ReplyKeyboardMarkup();

//            var keyboardRows = new List<KeyboardButton[]>();

//            foreach (var category in categories)
//            {
//                keyboardRows.Add(new KeyboardButton[] { category });
//            }

//            replyKeyboardMarkup.Keyboard = keyboardRows.ToArray();

//            await botClient.SendTextMessageAsync(chatId, "Select a category:", replyMarkup: replyKeyboardMarkup);
//        }

//        private static async Task SendTracks(long chatId, string category)
//        {
//            var tracks = GetTracksByCategory(category);

//            var response = $"Tracks in the '{category}' category:\n";

//            foreach (var track in tracks)
//            {
//                response += $"{track.Name} - {track.Artist}\n";
//            }

//            await botClient.SendTextMessageAsync(chatId, response);
//        }

//        private static List<Track> GetTracksByCategory(string category)
//        {
//            var tracks = new List<Track>();

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                connection.Open();

//                string query = "SELECT * FROM Track WHERE Category = @Category";
//                using (SqlCommand command = new SqlCommand(query, connection))
//                {
//                    command.Parameters.AddWithValue("@Category", category);

//                    using (SqlDataReader reader = command.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            var track = new Track
//                            {
//                                Name = reader["Name"].ToString(),
//                                Artist = reader["Artist"].ToString()
//                            };

//                            tracks.Add(track);
//                        }
//                    }
//                }
//            }

//            return tracks;
//        }
//    }

//    public class Track
//    {
//        public string Name { get; set; }
//        public string Artist { get; set; }
//    }
//}




