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

      public async Task<ServiceResponse<List<CoordinateDTO>>> GetLineRoute(int id)
      {
         ServiceResponse<List<CoordinateDTO>> response = new ServiceResponse<List<CoordinateDTO>>();
         List<CoordinateDTO> coordsDTO = new List<CoordinateDTO>();

         try
         {
            List<Coordinate> coordinates = await _context.Coordinates.Where(c => c.LineId == id).ToListAsync();
            foreach (var coord in coordinates)
            {
               coordsDTO.Add(new CoordinateDTO{ XCoordinate = coord.XCoordinate, YCoordinate = coord.YCoordinate });
            }
            response.Data = coordsDTO;
         }
         catch (Exception e) {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }

      public async Task<ServiceResponse<List<LineDTO>>> GetLines()
      {
         ServiceResponse<List<LineDTO>> response = new ServiceResponse<List<LineDTO>>();
         List<LineDTO> lineDTOs = new List<LineDTO>();
         try
         {
            List<Line> lines = await _context.Lines.Include(l => l.LineStations).ToListAsync();
            foreach (var line in lines)
            {
               LineDTO lineDTO = new LineDTO();
               lineDTO.Id = line.Id;
               lineDTO.Name = line.Name;
               lineDTO.Type = line.Type.ToString();
               lineDTO.StationIds = new List<int>();
               foreach (var item in line.LineStations)
               {
                  lineDTO.StationIds.Add(item.StationId);
               }
               lineDTOs.Add(lineDTO);
            }
            response.Data = lineDTOs;
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }

      public async Task<ServiceResponse<string>> AddLine(AddLineDTO newLine)
      {
         ServiceResponse<string> response = new ServiceResponse<string>();
         Enums.LineType type = (Enums.LineType) Enum.Parse(typeof(Enums.LineType), newLine.Type);
         List<Coordinate> coords = new List<Coordinate>();
         try
         {
            Line line = new Line{ Name = newLine.Name, Type = type };
            await _context.Lines.AddAsync(line);
            await _context.SaveChangesAsync();
            foreach (CoordinateDTO coord in newLine.Coordinates)
            {
               coords.Add(new Coordinate{LineId = line.Id, XCoordinate = coord.XCoordinate, YCoordinate = coord.YCoordinate});
            }
            await _context.Coordinates.AddRangeAsync(coords);
            await _context.SaveChangesAsync();
            response.Data = line.Name;
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }

      public async Task<ServiceResponse<List<LineDTO>>> UpdateLine(LineDTO newLine)
      {
         ServiceResponse<List<LineDTO>> response = new ServiceResponse<List<LineDTO>>();
         List<LineDTO> lineDTOs = new List<LineDTO>();
         LineDTO lineDTO = new LineDTO();
         List<LineStation> lineStations = new List<LineStation>();
         Enums.LineType type = (Enums.LineType) Enum.Parse(typeof(Enums.LineType), newLine.Type);
         
         foreach (int id in newLine.StationIds)
         {
            lineStations.Add(new LineStation { StationId = id, LineId = newLine.Id });
         }
         try
         {
            Line line = await _context.Lines.Include(l => l.LineStations).FirstOrDefaultAsync(l => l.Id == newLine.Id);
            _context.RemoveRange(line.LineStations);
            await _context.SaveChangesAsync();
            line.Name = newLine.Name;
            line.Type = type;
            line.LineStations = lineStations;
            _context.Lines.Update(line);
            await _context.SaveChangesAsync();

            List<Line> lines = await _context.Lines.Include(l => l.LineStations).ToListAsync();
            foreach (var temp in lines)
            {
               lineDTO.Id = temp.Id;
               lineDTO.Name = temp.Name;
               lineDTO.Type = temp.Type.ToString();
               lineDTO.StationIds = new List<int>();
               foreach (var item in temp.LineStations)
               {
                  lineDTO.StationIds.Add(item.StationId);
               }
               lineDTOs.Add(lineDTO);
            }
            response.Data = lineDTOs;
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }


      public async Task<ServiceResponse<int>> DeleteLine(int id)
      {
         ServiceResponse<int> response = new ServiceResponse<int>();

         try
         {
            Line line = await _context.Lines.FirstOrDefaultAsync(l => l.Id == id);
            _context.Lines.Remove(line);
            await _context.SaveChangesAsync();
            response.Data = line.Id; 
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

      public ServiceResponse<List<Coordinate>> GetCoordinates()
      {
         ServiceResponse<List<Coordinate>> response = new ServiceResponse<List<Coordinate>>();

         try
         {
            response.Data = _context.Coordinates.ToList();
            foreach(var tmp in response.Data)
            {
               Console.WriteLine(tmp.XCoordinate + "     " + tmp.YCoordinate);
            }
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