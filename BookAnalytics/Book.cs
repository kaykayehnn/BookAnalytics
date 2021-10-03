namespace _01_BookStatistics
{
    public struct Book
    {
        public Book(string title, string text)
        {
            this.Title = title;
            this.Text = text;
        }

        public string Title { get; }
        public string Text { get; }
    }
}
