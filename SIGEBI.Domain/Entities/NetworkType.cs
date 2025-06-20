namespace SIGEBI.Domain.Entities
{
    public class NetworkType
    {
       public int Id { get; set; }
        public string Name { get; set; }              // Nombre del tipo de red
        public string? Description { get; set; }      // Descripción opcional
        public bool IsActive { get; set; }            // Estado de activación
        public DateTime CreatedAt { get; set; }       // Fecha de creación
        public DateTime? UpdatedAt { get; set; }       // Fecha de última actualización
    }
}