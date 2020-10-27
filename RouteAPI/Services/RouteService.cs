using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
      private readonly IMapper _mapper;
      public RouteService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
      {
         _context = context;
         _httpContextAccessor = httpContextAccessor;
         _mapper = mapper;
      }

      public async Task<ServiceResponse<GetDeparturesDTO>> UpdateDeparture(AddDepartureDTO newDeparture)
      {
         ServiceResponse<GetDeparturesDTO> response = new ServiceResponse<GetDeparturesDTO>();
         List<DepartureDTO> departureDTOs = new List<DepartureDTO>();
         // Doesnt work without the date
         string time = "23.10.2020. " + newDeparture.Time;
         try
         {
            Departure departure = await _context.Departures.FirstOrDefaultAsync(d => d.Id == newDeparture.Id);
            DateTime tmp = Convert.ToDateTime(time);
            if (departure != null)
            {
               departure.Time = tmp;
               _context.Departures.Update(departure);
               await _context.SaveChangesAsync();

               Timetable timetable = await _context.Timetables
                  .Include(t => t.Departures)
                  .FirstOrDefaultAsync(t => t.Id == departure.TimetableId);
               timetable.Departures.OrderByDescending(d => d.Time);

               foreach (var dep in timetable.Departures)
               {
                  departureDTOs.Add(new DepartureDTO { Id = dep.Id, Time = dep.Time.ToString().Split(' ')[1].Substring(0, 5)});
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
         // Doesnt work without the date
         string time = "23.10.2020. " + newDeparture.Time;
         try
         {
            DateTime tmp = Convert.ToDateTime(time);
            await _context.Departures.AddAsync(
               new Departure
               {
                  Time = tmp,
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
                  departures.Add(new DepartureDTO { Id = dep.Id, Time = dep.Time.ToString().Split(' ')[1].Substring(0, 5) });
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

         try
         {
            Timetable timetable = await _context.Timetables
               .Include(t => t.Departures)
               .FirstOrDefaultAsync(t => t.Active == true && t.DayType == day && t.LineId == lineId);

            if (timetable != null)
            {
               foreach (var departure in timetable.Departures)
               {
                  departures.Add(new DepartureDTO
                     { 
                        Id = departure.Id,
                        // Removing the date part
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

      public async Task<ServiceResponse<List<Station>>> GetStations()
      {
         ServiceResponse<List<Station>> response = new ServiceResponse<List<Station>>();
         try
         {
            List<Station> stations = await _context.Stations.ToListAsync();
            //stations = await _context.Stations.Where(s => s.LineStations.Any(ls => ls.Line.Id == 1)).ToListAsync();
            response.Data = stations;
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }

      public async Task<ServiceResponse<List<StationDTO>>> GetStationNames()
      {
         ServiceResponse<List<StationDTO>> response = new ServiceResponse<List<StationDTO>>();
         List<StationDTO> stationDTOs = new List<StationDTO>();

         try
         {
            List<Station> stations = await _context.Stations.ToListAsync();
            foreach (var station in stations)
            {
               stationDTOs.Add(new StationDTO{ Id = station.Id, Name = station.Name });
            }
            response.Data = stationDTOs;
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }

      public async Task<ServiceResponse<List<Station>>> AddStation(AddStationDTO station)
      {
         ServiceResponse<List<Station>> response = new ServiceResponse<List<Station>>();
         
         try
         {
            await _context.Stations.AddAsync(new Station{ Name = station.Name, Address = station.Address, XCoordinate = station.XCoordinate, YCoordinate = station.YCoordinate});
            await _context.SaveChangesAsync();
            response.Data = await _context.Stations.ToListAsync();
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }

      public async Task<ServiceResponse<List<Station>>> UpdateStation(Station newStation)
      {
         ServiceResponse<List<Station>> response = new ServiceResponse<List<Station>>();
         try
         {
            Station station = await _context.Stations.FirstOrDefaultAsync(s => s.Id == newStation.Id);
            if (station != null)
            {
               station.Name = newStation.Name;
               station.Address = newStation.Address;
               station.XCoordinate = newStation.XCoordinate;
               station.YCoordinate = newStation.YCoordinate;

               _context.Stations.Update(station);
               await _context.SaveChangesAsync();
               response.Data = await _context.Stations.ToListAsync();
            }
            else
            {
               response.Success = false;
               response.Message = "Station with the given id was not found.";
            }
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }

      public async Task<ServiceResponse<List<Station>>> DeleteStation(int id)
      {
         ServiceResponse<List<Station>> response = new ServiceResponse<List<Station>>();
         try
         {
            Station station = await _context.Stations.FirstOrDefaultAsync(s => s.Id == id);
            _context.Stations.Remove(station);
            await _context.SaveChangesAsync();
            response.Data = await _context.Stations.ToListAsync();
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