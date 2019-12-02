using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerWebApi.Models;
using ShServer;
using ShServer.Models;

namespace WebApplication1.Controllers
{
    // [Route("api/[controller]")]
    // [ApiController]
    public class LogController : ControllerBase
    {
        public volatile static Dictionary<string, List<Log>> List = new Dictionary<string, List<Log>>();
        private LppDatabaseContext _ctx = new LppDatabaseContext();
        [HttpGet]
        public ActionResult getr(String namestring, String logstring)
        {
            LocationRoom LocRoom = new LocationRoom();
            LocRoom.Name = namestring;
            var ListLog = new List<Log>();
            String[] rawlog = logstring.Split("|");
            foreach (string str in rawlog)
            {
                if (str != "")
                {
                    String[] infomass = str.Split(" ");
                    Log log = new Log();
                    log.SSID = infomass[0];
                    log.BSSID = infomass[1];
                    log.AvgLevel = Convert.ToDouble(infomass[2]);
                    log.LocationId = LocRoom.Id;
                    log.LocationName = LocRoom.Name;
                    log.LocRomm = LocRoom;
                    ListLog.Add(log);
                }

            }
            if (List.ContainsKey(LocRoom.Name))
                foreach (Log lg in ListLog)
                {
                    if (List[LocRoom.Name].Find(x => x.BSSID.Contains(lg.BSSID)) != null)
                    {
                        List[LocRoom.Name].Find(x => x.BSSID.Contains(lg.BSSID)).AvgLevel += lg.AvgLevel;
                        List[LocRoom.Name].Find(x => x.BSSID.Contains(lg.BSSID)).number_of_Mentions++;
                        //xd[LocRoom].Find(x => x.BSSID.Contains(LogList.BSSID)).AvgLevel += Convert.ToDouble(wifiInfo.level.Split("dBm")[0]);
                        // xd[LocRoom].Find(x => x.BSSID.Contains(LogList.BSSID)).NumBerOfMentions++;
                    }
                    else
                        List[LocRoom.Name].Add(lg);
                }
            else
                List.Add(LocRoom.Name, ListLog);
            return Ok();
        }
        [HttpGet]
        public void Summ()
        {

            foreach (var keyvalue in List)
            {
                LocationRoom LocRoom = new LocationRoom();
                LocRoom.Name = keyvalue.Key;
                _ctx.LocationRooms.Add(LocRoom);
                _ctx.SaveChanges();
                foreach (var value in keyvalue.Value)
                {
                    Log log = new Log();
                    log.SSID = value.SSID;
                    log.BSSID = value.BSSID;
                    log.AvgLevel = value.AvgLevel / (value.number_of_Mentions + 1);
                    log.number_of_Mentions = value.number_of_Mentions;
                    log.LocationId = LocRoom.Id;
                    log.LocationName = LocRoom.Name;
                    log.LocRomm = LocRoom;
                    _ctx.Logs.Add(log);
                }
                _ctx.SaveChanges();
            }
        }
        [HttpGet]
        public string SummID(string Room)
        {
            bool flag = false;
            foreach (var keyvalue in List)
            {
                LocationRoom LocRoom = new LocationRoom();
                if (keyvalue.Key == Room)
                {
                    LocRoom.Name = keyvalue.Key;
                    _ctx.LocationRooms.Add(LocRoom);
                    _ctx.SaveChanges();
                    flag = true;
                    foreach (var value in keyvalue.Value)
                    {
                        Log log = new Log();
                        log.SSID = value.SSID;
                        log.BSSID = value.BSSID;
                        log.AvgLevel = value.AvgLevel / (value.number_of_Mentions + 1);
                        log.number_of_Mentions = value.number_of_Mentions;
                        log.LocationId = LocRoom.Id;
                        log.LocationName = LocRoom.Name;
                        log.LocRomm = LocRoom;
                        _ctx.Logs.Add(log);
                    }
                    _ctx.SaveChanges();
                }
            }
            if (flag == true)
                return "Add ID";
            else return "No ID";
        }
        [HttpGet]
        public List<Log> Getting(int Id)
        {
            LppDatabaseContext db = new LppDatabaseContext();

            var logs = (from item in db.Logs
                        where item.LocationId == Id
                        select item).ToList();
            return logs;
        }
        [HttpGet]
        public string GettingString(int Id)
        {
            string result = "";
            LppDatabaseContext db = new LppDatabaseContext();

            var logs = (from item in db.Logs
                        where item.LocationId == Id
                        select item).ToList();
            foreach (var item in logs)
            {
                result = result + item.SSID.ToString() + " " + item.BSSID.ToString() + " " + item.AvgLevel.ToString() + '\n';
            }
            return result;
        }

        [HttpGet]
        public string answer(string str)
        {
            int a = Int32.Parse(str);
            if (a > 100)
                return "a<0";
            else return "a>0";
        }

        [HttpGet]
        public string PointCount()  //возвращает количество точек в БД
        {
            int i = 0;
            LppDatabaseContext db = new LppDatabaseContext();
            foreach (var item in db.LocationRooms)
                i++;
            return i.ToString();
        }

        [HttpGet]
        public string NameListPoint() //возвращает список точек и их ID
        {
            string Name_ListPoint = "";
            LppDatabaseContext db = new LppDatabaseContext();
            foreach (var item in db.LocationRooms)
            {
                Name_ListPoint = Name_ListPoint + item.Id.ToString() + " " + item.Name + '\n';
            }
            return Name_ListPoint;
        }

        [HttpGet]
        public ActionResult addpoint(String namestring, String logstring)
        {
            LocationRoom LocRoom = new LocationRoom();
            LocRoom.Name = namestring;
            // var ListLog = new List<Log>();
            _ctx.LocationRooms.Add(LocRoom);
            _ctx.SaveChanges();
            String[] rawlog = logstring.Split("|");
            foreach (string str in rawlog)
            {
                if (str != "")
                {
                    String[] infomass = str.Split(" ");
                    Log log = new Log();
                    log.SSID = infomass[0];
                    log.BSSID = infomass[1];
                    log.AvgLevel = Convert.ToDouble(infomass[2]);
                    log.LocationId = LocRoom.Id;
                    log.LocationName = LocRoom.Name;
                    log.LocRomm = LocRoom;
                    // ListLog.Add(log);
                    _ctx.Logs.Add(log);
                }

            }
            _ctx.SaveChanges();
            return Ok();
        }
        public string NameListLog() //возвращает список логов и их ID
        {
            string Name_ListLog = "";
            LppDatabaseContext db = new LppDatabaseContext();
            foreach (var item in db.Logs)
            {
                Name_ListLog = Name_ListLog + item.Id.ToString() + " " + item.LocationId.ToString()+" "+item.LocationName+" " + " "+item.SSID.ToString()+" " + item.BSSID.ToString()+
                    " "+item.AvgLevel.ToString() + '\n';
            }
            return Name_ListLog;
        }
        public string GettingName(string Name)
        {
            string result = "";
            LppDatabaseContext db = new LppDatabaseContext();

            var logs = (from item in db.Logs
                        where item.LocationName == Name
                        select item).ToList();
            foreach (var item in logs)
            {
                result = result + item.SSID.ToString() + " " + item.BSSID.ToString() + " " + item.AvgLevel.ToString() + '\n';
            }
            return result;
        }

    }
}