struct User
{
    public long Id;
    public string Name;
    public int Step;

    public User(long id, string name, int step)
    {
        Id = id;
        Name = name;
        Step = step;
    }
    public void SetStep(int step)
    {
        Step = step;
    }

    public string ToText()
    {
        return $"{Id}, Ismi: {Name}, Step: {Step}";
    }
}