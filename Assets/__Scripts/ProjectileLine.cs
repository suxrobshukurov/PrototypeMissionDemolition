using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; // Одиночка

    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    private void Awake()
    {
        S = this; // установить ссылку на объект-одиночку
        line = GetComponent<LineRenderer>(); // Получить ссылку на LineRenderer
        line.enabled = false; // выключить LineRenderer, пока он не понадобится
        points = new List<Vector3>(); // инициализировать список точек
    }

    // это свойство (то есть метод, маскирующийся вод поле)
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                // если поле _poi содержит действительную ссылку,
                // сбросить все отсальные парпметры в исходное состояние
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    // этот метод модно вызвать непосредственно, чтобы стереть линию
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        // Вызывается для добавления точки в линии 
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            // если точка недостаточно далеко от предыдущей, просто выйти
            return;
        }
        if (points.Count == 0)
        {
            // Если это точка запуска...
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; // Для определения
            // добавить дополнительный фрагмент линии, чтобы помочь лучше прицелиться в будущем
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            // установить первые две точки
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            // включить LineRenderer
            line.enabled = true;
        }
        else
        {
            // обычная последовательность добавления точек
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    // Возвращает местоположение последней добваленной точки
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {// если точек нет, вернуть vector3.zero
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }

    private void FixedUpdate()
    {
        if (poi == null)
        {
            // Если свойство poi содержит пустое значение, найти интересующий объект
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return; // Выйти, если интересующий объект не найден
                }
            }
            else
            {
                return; // Выйти, если интересующий объект не найден
            }
        }
        // если интересуещий объект найден,
        // попытаться добавить точку с его кординатами в каждом кадре FixedUpdate
        AddPoint();
        if (poi.GetComponent<Rigidbody>().IsSleeping())
        {
            //Если FollowCam.POI содержит null, записать null в poi
            poi = null;
        }
    }
}
