
using System.ComponentModel.DataAnnotations.Schema;

public class Sheikh
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        [NotMapped]
        public List<FileInfo> SurahAudioData { get; set; }
    }
