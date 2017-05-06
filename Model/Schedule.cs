using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schedule {

    public Dictionary<int, ScheduleType> scheduleMap;


    public Schedule()
    {
        scheduleMap = new Dictionary<int, ScheduleType>();

        for (int i = 0; i < 24; i++)
        {
            if(i > 6 && i < 22)
            {
                scheduleMap.Add(i, ScheduleType.Free);
            }
            else
            {
                scheduleMap.Add(i, ScheduleType.Rest);
            }
        }
    }
}
