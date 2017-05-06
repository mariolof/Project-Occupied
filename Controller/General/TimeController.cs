using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeController : MonoBehaviour {

    public static TimeController Instance { get; protected set; }

    public List<Schedule> scheduleList { get; protected set; }

    public int year { get; protected set; }
    public int month { get; protected set; }
    public int date { get; protected set; }
    public int hour { get; protected set; }

    public float currentTime { get; protected set; }
    public float cycle { get; protected set; }


    void OnEnable()
    {
        Instance = this;

        scheduleList = new List<Schedule>();

        Time.timeScale = 1;
    }


    void Start ()
    {
        year = 2941;
        month = 3;
        date = 16;
        hour = 6;
        cycle = 10f;

        AddSchedule();
    }
	

	void Update ()
    {
        TimeCycle();
    }


    public void TimeCycle()
    {
        if (currentTime < cycle)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            currentTime = 0f;
            UpdateEveryHour();

            if (hour < 23)
            {
                hour += 1;
            }
            else
            {
                hour = 0;
                UpdateEveryDay();

                if (date < 30)
                {
                    date += 1;
                }
                else
                {
                    date = 1;

                    if (month < 12)
                    {
                        month += 1;
                    }
                    else
                    {
                        month = 1;
                        year += 1;
                    }
                }
            }
        }
    }


    public void UpdateEveryHour()
    {
        PlantController.Instance.PlantGrow();
        NeedController.Instance.ReduceHunger();
    }


    public void UpdateEveryDay()
    {

    }


    public void AddSchedule()
    {
        if(scheduleList.Count < 5)
        {
            Schedule schedule = new Schedule();

            scheduleList.Add(schedule);
            SchedulePanelController.Instance.AddScheduleInfo(schedule);
            VillagerPanelController.Instance.AddScheduleDropDownOption(schedule);
        }
    }


    public void SetTimeScale(float timescale)
    {
        Time.timeScale = timescale;
    }
}
