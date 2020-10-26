using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RouteAPI.Data;
using RouteAPI.Models;

namespace RouteAPI.Services
{
   public class RouteService : IRouteService
   {
      private readonly DataContext _context;
      private readonly IHttpContextAccessor _httpContextAccessor;
      public RouteService(DataContext context, IHttpContextAccessor httpContextAccessor)
      {
         _context = context;
         _httpContextAccessor = httpContextAccessor;
      }

      public async Task<ServiceResponse<GetDeparturesDTO>> UpdateDeparture(AddDepartureDTO newDeparture)
      {
         ServiceResponse<GetDeparturesDTO> response = new ServiceResponse<GetDeparturesDTO>();
         List<DepartureDTO> departureDTOs = new List<DepartureDTO>();
         try
         {
            Departure departure = await _context.Departures.FirstOrDefaultAsync(d => d.Id == newDeparture.Id);
            if (departure != null)
            {
               departure.Time = Convert.ToDateTime(newDeparture.Time);
               _context.Departures.Update(departure);
               await _context.SaveChangesAsync();

               Timetable timetable = await _context.Timetables
                  .Include(t => t.Departures)
                  .FirstOrDefaultAsync(t => t.Id == departure.TimetableId);
               timetable.Departures.OrderByDescending(d => d.Time);

               foreach (var dep in timetable.Departures)
               {
                  departureDTOs.Add(new DepartureDTO { Id = dep.Id, Time = dep.Time.ToString()});
               }

               response.Data = new GetDeparturesDTO { TimetableId = timetable.Id, Departures = departureDTOs };
            }
            else
            {
               response.Success = false;
               response.Message = "Departure with given id was not found.";
            }
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }
      public async Task<ServiceResponse<GetDeparturesDTO>> AddDeparture(AddDepartureDTO newDeparture)
      {
         ServiceResponse<GetDeparturesDTO> response = new ServiceResponse<GetDeparturesDTO>();
         List<DepartureDTO> departures = new List<DepartureDTO>();
         DateTime tmp = Convert.ToDateTime(newDeparture.Time);
         try
         {
            await _context.Departures.AddAsync(
               new Departure
               {
                  Time = Convert.ToDateTime(newDeparture.Time),
                  TimetableId = newDeparture.TimetableId 
               }
            );
            await _context.SaveChangesAsync();

            Timetable timetable = await _context.Timetables
               .Include(t => t.Departures)
               .FirstOrDefaultAsync(t => t.Id == newDeparture.TimetableId);

            // Testirati ovo
            timetable.Departures.OrderByDescending(d => d.Time);

            if (timetable != null)
            {
               foreach (var dep in timetable.Departures)
               {
                  departures.Add(new DepartureDTO { Id = dep.Id, Time = dep.Time.ToString() });
               }
               response.Data = new GetDeparturesDTO { TimetableId = timetable.Id, Departures = departures };
            }
            else
            {
               response.Success = false;
               response.Message = "Timetable not found.";
            }
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }

         return response;
      }

      public async Task<ServiceResponse<GetDeparturesDTO>> GetDepartures(string dayType, int lineId)
      {
         ServiceResponse<GetDeparturesDTO> response = new ServiceResponse<GetDeparturesDTO>();
         Enums.DayType day = (Enums.DayType) Enum.Parse(typeof(Enums.DayType), dayType);
         //Enums.LineType line = (Enums.LineType) Enum.Parse(typeof(Enums.LineType), lineType);
         List<DepartureDTO> departures = new List<DepartureDTO>();
         string formattedTime;

         try
         {
            Timetable timetable = await _context.Timetables
               .Include(t => t.Departures)
               .FirstOrDefaultAsync(t => t.Active == true && t.DayType == day && t.LineId == lineId);

            //timetable.Departures.OrderByDescending(d => d.Time);
            if (timetable != null)
            {
               foreach (var departure in timetable.Departures)
               {
                  departures.Add(new DepartureDTO
                     { 
                        Id = departure.Id,
                        Time = departure.Time.ToString().Split(' ')[1].Substring(0, 5)
                     }
                  );
               }
               foreach (var departure in timetable.Departures)
               {
                  departures.Add(new DepartureDTO
                     { 
                        Id = departure.Id,
                        Time = departure.Time.ToString().Split(' ')[1].Substring(0, 5)
                     }
                  );
               }
               departures.OrderByDescending(d => d.Time);
               response.Data = new GetDeparturesDTO { TimetableId = timetable.Id, Departures = departures };
            }
            else
            {
               response.Success = false;
               response.Message = "Timetable not found.";
            }
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }

      public async Task<ServiceResponse<int>> DeleteDeparture(int id)
      {
         ServiceResponse<int> response = new ServiceResponse<int>();
         try
         {
            Departure departure = await _context.Departures.FirstOrDefaultAsync(d => d.Id == id);
            _context.Departures.Remove(departure);
            await _context.SaveChangesAsync();
            response.Data = id;
         }
         catch (Exception e)
         {
            response.Success = true;
            response.Message = e.Message;
         }
         return response;
      }

      public async Task<ServiceResponse<List<LineNameDTO>>> GetLineNames(string dayType, string lineType)
      {
         ServiceResponse<List<LineNameDTO>> response = new ServiceResponse<List<LineNameDTO>>();
         List<LineNameDTO> lineNames = new List<LineNameDTO>();

         Enums.DayType day = (Enums.DayType) Enum.Parse(typeof(Enums.DayType), dayType);
         Enums.LineType line = (Enums.LineType) Enum.Parse(typeof(Enums.LineType), lineType);
         try
         {
            List<Line> lines = await _context.Lines.ToListAsync();
            foreach (Line temp in lines)
            {
               lineNames.Add(new LineNameDTO {Id = temp.Id, Name = temp.Name});
            }
            response.Data = lineNames;
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }
   }
}