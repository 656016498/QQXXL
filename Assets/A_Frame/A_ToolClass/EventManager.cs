using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MEventType { 
    PassConditon,
    LevelNextStep,
    SortType,
    WaterMove,
    BallAddFore,
    RefreshTresure,
}

//public delegate T EventDele(T ); 
public class EventManager : Singlton<EventManager>
{
    
    public delegate void EventDelegate(object[] args);

    private Dictionary<MEventType, Dictionary<int, List<EventDelegate>>> eventListeners = new Dictionary<MEventType, Dictionary<int, List<EventDelegate>>>();


    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="eventDelegate"></param>
    public void AddEvent(MEventType eventType,EventDelegate eventDelegate) {
        AddSubEvent(eventType,-1, eventDelegate);
    }
    public void AddSubEvent(MEventType eventType,int SubType,EventDelegate eventDelegate) 
    {
        Dictionary<int, List<EventDelegate>> tempEventList = null;
        //List<EventDelegat>
        if (eventListeners.TryGetValue(eventType, out tempEventList))
        {
            List<EventDelegate> eventList = null;
            if (!tempEventList.TryGetValue(SubType, out eventList))
            {
                eventList = new List<EventDelegate>();
                eventList.Add(eventDelegate);
                tempEventList.Add(SubType, eventList);
            }
            else
            {
                if (!eventList.Contains(eventDelegate))
                {
                    eventList.Add(eventDelegate);
                }
            }
        }
        else {
            tempEventList = new Dictionary<int, List<EventDelegate>>();
            List<EventDelegate> eventList = new List<EventDelegate>();
            eventList.Add(eventDelegate);
            tempEventList.Add(SubType, eventList);
            eventListeners.Add(eventType, tempEventList);
        }


    }

    public void RemoveEvetn(MEventType eventType,EventDelegate eventDelegate)
    {
        RemoveSubEvent(eventType, -1,eventDelegate);
    }


    public void RemoveSubEvent(MEventType eventType, int subType, EventDelegate eventDelegate)
    {
        Dictionary<int, List<EventDelegate>> tempEventList = null;
        if (eventListeners.TryGetValue(eventType, out tempEventList))
        {
            List<EventDelegate> eventlist;
            if (tempEventList.TryGetValue(subType, out eventlist))
            {
                eventlist.Remove(eventDelegate);
            }
        }
    }

    /// <summary>
    /// 执行事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="args"></param>
    public void ExecuteEvent(MEventType eventType, params object[] args)
    {
        ExecuteSubEvent(eventType, -1, args);
    }
    /// <summary>
    /// 执行事件
    /// </summary>
    public void ExecuteSubEvent(MEventType eventType, int SubType, params object[] args) {
        Dictionary<int, List<EventDelegate>> tempEventList = null;
        if (eventListeners.TryGetValue(eventType,out tempEventList))
        {
            List<EventDelegate> eventList = null;
            if (SubType < 0)
            {
                foreach (var item in tempEventList)
                {
                    if (item.Value != null)
                    {
                        foreach (var value in item.Value)
                        {
                            if (value != null)
                            {
                                value.Invoke(args);
                            }

                        }
                    }
                }
            }
            else if(tempEventList.TryGetValue(SubType,out eventList)){

                foreach (var item in eventList)
                {
                    if (item!=null)
                    {
                        item.Invoke(args);
                    }
                }
            
            }
        }
    

    }


    /// <summary>
    /// 清空
    /// </summary>
    public void Claen() {

        eventListeners.Clear();
    
    }




}
