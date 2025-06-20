namespace SIGEBI.Domain.Entities
{
    public class NetworkType
    {
       public int Id { get; set; }
        public string Name { get; set; }              // Nombre del tipo de red
        public string? Description { get; set; }      // Descripci�n opcional
        public bool IsActive { get; set; }            // Estado de activaci�n
        public DateTime CreatedAt { get; set; }       // Fecha de creaci�n
        public DateTime? UpdatedAt { get; set; }       // Fecha de �ltima actualizaci�n
    }
}