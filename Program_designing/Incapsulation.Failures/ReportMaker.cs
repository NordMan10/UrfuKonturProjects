using System;
using System.Collections.Generic;
using System.Linq;

namespace Incapsulation.Failures
{
    public class Common
    {
        public static int Earlier(object[] v, int day, int month, int year)
        {
            var failureDateTime = new DateTime((int)v[2], (int)v[1], (int)v[0]);
            var referenceDateTime = new DateTime(year, month, day);

            if (failureDateTime < referenceDateTime) return 1;
            return 0;
        }
    }

    public class Device
    {
        public Device(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != null)
                    name = value;
            }
        }
    }

    public enum FailureType
    {
        UnexpectedShutdown = 0,
        ShortNonResponding,
        HardwareFailures,
        ConnectionProblems
    }

    public class Failure
    {
        public Failure(FailureType type, DateTime time, int deviceId) 
        { 
            Type = type;
            Time = time;
            DeviceId = deviceId;
        }

        public FailureType Type { get; set; }

        public DateTime Time { get; }

        public int DeviceId { get; }

        public bool IsFailureSerious()
        {
            return (int)Type == 0 || (int)Type == 2;
        }
    }

    public class ReportMaker
    {
        /// <summary>
        /// </summary>
        /// <param name="day"></param>
        /// <param name="failureTypes">
        /// 0 for unexpected shutdown, 
        /// 1 for short non-responding, 
        /// 2 for hardware failures, 
        /// 3 for connection problems
        /// </param>
        /// <param name="deviceId"></param>
        /// <param name="times"></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes, 
            int[] deviceId, 
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            var deviceList = new List<Device>();
            var failureList = new List<Failure>();

            for (var i = 0; i < devices.Count; i++)
            {
                var device = GetDevice(devices[i]);

                var failureIndex = GetFailureIndex(deviceId, device);

                if (failureIndex >= 0)
                    failureList.Add(new Failure(GetFailureType(failureTypes[failureIndex]), 
                        GetDateTimeFromObjectTime(times[failureIndex]), device.Id));

                deviceList.Add(device);
            }

            var result = FindDevicesFailedBeforeDate(deviceList, failureList, new DateTime(year, month, day));

            return result;
        }

        private static List<string> FindDevicesFailedBeforeDate(
            List<Device> devices, List<Failure> failures, DateTime referenceDate)
        {
            var seriousFailures = failures.Where(failure => failure.IsFailureSerious()).ToList();
            var problematicDevicesId = new List<int>();
            for (var i = 0; i < seriousFailures.Count; i++)
            {
                if (seriousFailures.Count > 0 && IsFailureBeforeDate(seriousFailures[i], referenceDate))
                    problematicDevicesId.Add(seriousFailures[i].DeviceId);
            }

            var problematicDevices = devices.Where(device => problematicDevicesId.Contains(device.Id));

            return problematicDevices.Select(device => device.Name).ToList();
        }

        private static Device GetDevice(Dictionary<string, object> device)
        {
            return new Device((int)device["DeviceId"], device["Name"] as string);
        }

        private static int GetFailureIndex(int[] deviceId, Device device)
        {
            var result = deviceId.Where(id => id == device.Id).ToList();
            if (result.Count > 0) return result.First();
            return -1;
        }

        private static bool IsFailureBeforeDate(Failure failure, DateTime referenceDate)
        {
            return Common.Earlier(GetObjectTimeFromDateTime(failure.Time), 
                referenceDate.Day, referenceDate.Month, referenceDate.Year) == 1;
        }

        private static FailureType GetFailureType(int failureType)
        {
            if (Enum.IsDefined(typeof(FailureType), failureType))
                return (FailureType)failureType;
            else throw new ArgumentException();
        }

        private static DateTime GetDateTimeFromObjectTime(object[] time)
        {
            return new DateTime((int)time[2], (int)time[1], (int)time[0]);
        }

        private static object[] GetObjectTimeFromDateTime(DateTime dateTime)
        {
            return new object[] { dateTime.Day, dateTime.Month, dateTime.Year };
        }
    }
}
