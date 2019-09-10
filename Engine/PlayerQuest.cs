namespace Engine
{
    public class PlayerQuest
    {
        public Quest Details { get; set; }
        public bool isCompleted { get; set; }

        public PlayerQuest(Quest details)
        {
            Details = details;
            isCompleted = false;
        }
    }
}