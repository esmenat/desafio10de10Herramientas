using AutoMapper;
using Sistema.Modelos;
using Sistema.Modelos.Modelos;
using Sistema.Modelos.DTOs; // Asegúrate de tener los modelos correctos
                               // Asegúrate de tener los DTOs correctos

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Configuración de mapeo
        CreateMap<Camion, CamionDTO>(); // Mapea el modelo Camion al DTO CamionDTO
        CreateMap<Conductor, ConductorDTO>(); // Mapea el modelo Conductor al DTO ConductorDTO
        CreateMap<Licencia, LicenciaDTO>(); // Mapea el modelo Licencia al DTO LicenciaDTO
        CreateMap<MantenimientoProgramado, MantenimientoProgramadoDTO>(); // Mapea el modelo MantenimientoProgramado al DTO MantenimientoProgramadoDTO
        CreateMap<Taller, TallerDTO>(); // Mapea el modelo Taller al DTO TallerDTO
        CreateMap<Usuario, UsuarioDTO>(); // Mapea el modelo Usuario al DTO UsuarioDTO
    }
}
