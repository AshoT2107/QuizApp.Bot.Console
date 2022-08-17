struct Question
{
    public string QuestionText;
    public int CorrectAnswerIndex;
    public List<string> Choices;

    public Question(string question, int index, List<string> choices)
    {
        QuestionText = question;
        CorrectAnswerIndex = index;
        Choices = choices;
    }

    public string GetCorrectAnswer()
    {
        return Choices[CorrectAnswerIndex];
    }
}