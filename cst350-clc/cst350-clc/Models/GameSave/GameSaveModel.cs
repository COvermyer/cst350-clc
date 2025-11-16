namespace cst350_clc.Models.GameSave
{
    /// <summary>
    /// Model class for a Game Save
    /// </summary>
    public class GameSaveModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateSaved { get; set; }
        public string SaveData { get; set; }
    }
}
