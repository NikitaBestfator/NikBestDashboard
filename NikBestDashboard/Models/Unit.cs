public class Unit
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Game { get; set; } = string.Empty;        // Майнкрафт, Terraria, Diablo...
    public string Category { get; set; } = "Моб";           // Моб, Босс, NPC, Герой
    public int Health { get; set; }                         // Здоровье
    public int Attack { get; set; }                         // Сила атаки
    public int Defense { get; set; }                        // Защита
    public double Speed { get; set; }                       // Скорость
    public string AttackType { get; set; } = "Ближняя";     // Ближняя, Дальняя, Магия
    public string Description { get; set; } = string.Empty; // Особенности
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}