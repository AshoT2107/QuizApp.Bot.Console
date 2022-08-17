using Telegram.Bot;
using Telegram.Bot.Types;

List<Question> questions = new List<Question>();
AddDefaultQuestion();

Dictionary<long, User> users = new Dictionary<long, User>();

string botToken = "5226260727:AAGoeucHz3nOlDlTs1YIHL3y92Yi93eWV-A";
TelegramBotClient bot = new TelegramBotClient(botToken);
bot.StartReceiving(
    updateHandler: (client, update, token) => GetUpdate(update),
    errorHandler: (client, exception, token) => Task.CompletedTask);
Console.ReadKey();

async Task GetUpdate(Update update)
{
    Console.WriteLine(update.Message.Text);
    var text = update.Message.Text;
    var chatId = update.Message.Chat.Id;
    if (!users.ContainsKey(chatId))
    {
        string name = string.IsNullOrEmpty(update.Message.From.Username)
            ? update.Message.From.FirstName
            : "@" + update.Message.From.Username;

        SaveUser(chatId, name, 0);
    }
    else
    {
        if (users[chatId].Step == 2)
        {
            List<string> choises = new List<string>();
            var messages = text.Split(',').ToList();
            for (int i = 0; i < messages.Count; i++)
            {
                if (i > 1)
                {
                    choises.Add(messages[i]);
                }
            }
            var newQuestion = new Question(messages[0], int.Parse(messages[1]), choises);
            SaveQuestion(chatId,newQuestion);
        }
    }
    //if(users.step== 2)
    //text savol deb qabul split Questiondan obyekt yaratib questionsga qoshamiz

    switch (text)
    {
        case "menu": ShowMenu(chatId); break;
        case "2": AddQuestion(chatId); break;
        case "3": ShowDashboard(chatId); break;
        case "5": ShowUsers(chatId); break;
    }
}

void SendMessage(long chatId, string messageText)
{
    bot.SendTextMessageAsync(chatId, messageText);
}

void ShowMenu(long chatId)
{
    var menu = new List<EMenu>()
    {
        EMenu.StartQuiz,
        EMenu.AddQuestion,
        EMenu.Dashboard,
        EMenu.Statistics,
        EMenu.Users,
        EMenu.Close
    };

    string menuText = "";
    foreach (var eMenu in menu)
    {
        menuText += $"{(int)eMenu}. {eMenu}\n";
    }

    SendMessage(chatId, menuText);
}

void AddDefaultQuestion()
{
    questions.Add(new Question("1 + 2 = ?", 1, new List<string>() { "2", "3", "12", "32" }));
    questions.Add(new Question("1 * 2 = ?", 2, new List<string>() { "21", "34", "2", "32" }));
    questions.Add(new Question("4 / 2 = ?", 0, new List<string>() { "2", "13", "12", "32" }));
}

void ShowDashboard(long chatId)
{
    string message = "Savollar " + questions.Count + " ta.\n";
    string questionsText = "";

    for (int i = 0; i < questions.Count; i++)
    {
        questionsText += $"{i + 1}. {questions[i].QuestionText}\n";
    }

    message += questionsText;
    message += "\nMenuga qaytish uchun 'menu' deb kiriting";
    SendMessage(chatId, message);
}

void AddQuestion(long chatId)
{
    string addQuestionText = "Savolni quyidagi tartibda kiriting : ";
    addQuestionText += "1 + 4 = ?, 2, 12, 14, 5, 6";
    SendMessage(chatId, addQuestionText);
    SetStep(chatId, 2);
    //setstep(2)
}

void SetStep(long id, int step)
{
    var user = users[id];
    user.SetStep(step);
    users[id] = users[id];
}

void SaveQuestion(long chatId,Question newQuestion)
{
    questions.Add(newQuestion);
    SetStep(chatId, 0);
    SendMessage(chatId, "Savol qo'shildi");
    ShowDashboard(chatId);
}
void SaveUser(long chatId, string name, int step)
{
    users.Add(chatId, new User(chatId, name, step));
}

void ShowUsers(long chatId)
{
    var message = "Foydalanuvchilar royxati: \n";

    var usersText = "";
    for (int i = 0; i < users.Values.Count; i++)
    {
        usersText += users.Values.ElementAt(i).ToText() + "\n";
    }

    message += usersText;

    SendMessage(chatId, message);
}