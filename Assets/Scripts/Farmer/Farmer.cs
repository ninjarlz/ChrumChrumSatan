﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour {

 /*   public int Width = 18, Height = 10;
    public Queue<Field> Frontier { get; set; }
    public Dictionary<Field, Field> CameFrom { get; set; }
    public Dictionary<Field, int> FromDirection { get; set; }
    public List<Field> Path { get; set; }
    [SerializeField]
    private Field[] _fieldsToVisit;
    public Field CurrentDestination { get; set; }
    [SerializeField]
    private Field _currentField;
    public Field CurrentField
    {
        get
        {
            return _currentField;
        }
        set
        {
            _currentField = value;
        }
    }
    public Field CurrentTarget { get; set; }

    private bool _achievedFirstField = false;

    
    [SerializeField]
    private Direction _currentRotation = Direction.N;

    [SerializeField]
    private int _movementSpeed;
    [SerializeField]
    private int _rotatingSpeed;

    public enum MovementState { Moving, Rotating }
    [SerializeField]
    private MovementState _currentState = MovementState.Moving;
    public MovementState CurrentState { get { return _currentState; } set { _currentState = value; } }
    
    private delegate void currentState();
    private currentState _currentStateDelegate;

    private Quaternion _destinationRotation;

    public Field[,] Fields { get; set; }
    private Cultist[] _cultists;

    void ActivateFields()
    {
        Fields = new Field[Width, Height];
        int y = 0, x = 0;
        GameObject 
        for (int y = -Height / 2; y <= Height / 2 - 1; y++)
            for (int x = -Width / 2; x <= Width / 2 - 1; x++)
            {
                GameObject field = new GameObject { name = "Field " + (x + Width / 2).ToString() + " " + (y + Height / 2).ToString() };
                field.transform.SetParent(transform);
                Field fieldScript = field.AddComponent<Field>();
                fieldScript.X = x + Width / 2;
                fieldScript.Y = y + Height / 2;
                fieldScript.Width = Width;
                fieldScript.Height = Height;
                Fields[fieldScript.X, fieldScript.Y] = fieldScript;
                fieldScript.Width = Width;
                fieldScript.Height = Height;
                field.transform.position = new Vector2(2 * x + 1, 2 * y + 1);
            }

        foreach (Field field in Fields)
        {
            int x = field.X;
            int y = field.Y;
            if (y + 1 < Height)
            {
                field.Neighbors[0] = Fields[x, y + 1];
                if (x + 1 < Width) field.Neighbors[1] = Fields[x + 1, y + 1];
                if (x - 1 >= 0) field.Neighbors[7] = Fields[x - 1, y + 1];
            }
            if (y - 1 >= 0)
            {
                field.Neighbors[4] = Fields[x, y - 1];
                if (x + 1 < Width) field.Neighbors[3] = Fields[x + 1, y - 1];
                if (x - 1 >= 0) field.Neighbors[5] = Fields[x - 1, y - 1];
            }
            if (x + 1 < Width) field.Neighbors[2] = Fields[x + 1, y];
            if (x - 1 >= 0) field.Neighbors[6] = Fields[x - 1, y];
        }
    }

    void Awake()
    {
        ActivateFields();
        
        Frontier = new Queue<Field>();
        CameFrom = new Dictionary<Field, Field>();
        FromDirection = new Dictionary<Field, int>();
        Path = new List<Field>();
        _currentStateDelegate = Moving;
        transform.position = _currentField.transform.position;
        //Debug.Log(_currentField.XOffset + 1 + " " + _currentField.YOffset + 1);
        if (Fields == null) Debug.Log("AAA");
        FindPath(Fields[_currentField.X, _currentField.Y + 1]);
    }

	void Start ()
    {
        //_cultists[0] = GameObject.Find("Cultist 1").GetComponent<Cultist>();
	}
	
	
	void Update ()
    {
        _currentStateDelegate();
	}

    void Moving()
    {
        transform.position = Vector3.MoveTowards(transform.position, Path[0].transform.position, _movementSpeed * Time.deltaTime);
        if (transform.position == Path[0].transform.position) OnArrivalToField(Path[0]);
    }

    void Rotating()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _destinationRotation, _rotatingSpeed * Time.deltaTime);
        if (transform.rotation == _destinationRotation)
        {
            if (Path.Count != 0)
            {
                _currentStateDelegate = Moving;
                _currentState = MovementState.Moving;
            }
            else
            {
                Field random = FindRandomField();
                FindPath(random);
                if (_currentField.GetNeighbor(_currentRotation) == Path[0])
                {
                    _currentState = MovementState.Rotating;
                    _currentStateDelegate = Rotating;
                }
                else
                {
                    _currentState = MovementState.Moving;
                    _currentStateDelegate = Moving;
                }
            }
        }
    }

    void OnArrivalToField(Field field)
    {
        if (_achievedFirstField)
        {
            _currentField = field;
            Path.RemoveAt(0);
        }
        else _achievedFirstField = true;

        if (Path.Count == 0)
        {
            //Animator.SetBool("Walking", false);
            Field random = FindRandomField();
            FindPath(random);
            if (_currentField.GetNeighbor(_currentRotation) == Path[0])
            {
                _currentState = MovementState.Rotating;
                _currentStateDelegate = Rotating;
            }
            else
            {
                _currentState = MovementState.Moving;
                _currentStateDelegate = Moving;
            }
        }
        else
        {
            if (_currentField.GetNeighbor(_currentRotation) == Path[0])
            {
                _currentState = MovementState.Moving;
                _currentStateDelegate = Moving;
            }
            else Rotate(Path[0]);
        }
    }

    void Rotate(Field field)
    {
        _currentState = MovementState.Rotating;
        _currentStateDelegate = Rotating;
        _destinationRotation = GetRotation(_currentRotation, _currentField, field);
    }

    Quaternion GetRotation(Direction currentRotation, Field currentField, Field neighbor)
    {
        Direction nextDirection = _currentField.GetDirection(neighbor);
        return GetRotation(_currentRotation, nextDirection);
    }


    Quaternion GetRotation(Direction currentDirection, Direction nextDirection)
    {
        return Quaternion.Euler(0, (int)_currentRotation * 45 + currentDirection.SmallestDifference(nextDirection) * 45, 0);
    }
    

    void FindPath(Field field)
    {
        _achievedFirstField = false;
        Frontier.Clear();
        CameFrom.Clear();
        Path.Clear();
        FromDirection.Clear();
        Frontier.Enqueue(_currentField);
        CameFrom.Add(_currentField, null);
        FromDirection.Add(_currentField, (int)_currentRotation);

        while (Frontier.Count != 0)
        {
            Field current = Frontier.Dequeue();
            if (current == field) { Debug.Log("chuj"); break; }
            for (Direction direction = Direction.N; direction < Direction.NW; direction++)
            {
                Field neighbor = current.Neighbors[(int)direction];
                if (neighbor && !CameFrom.ContainsKey(neighbor))
                {
                    Debug.Log(neighbor.X + " " + neighbor.Y);
                    if (neighbor == field) Debug.Log("Tu cie mam");
                    FromDirection[neighbor] = (int)direction;
                    Frontier.Enqueue(neighbor);
                    CameFrom[neighbor] = current;
                }
            }
        }

        Field curr = field;
        while (curr != _currentField)
        {
            Path.Add(curr);
            curr = CameFrom[curr];
        }
        Path.Reverse();
    }

    Field FindRandomField()
    {
        Field randomField = _currentField;
        while (randomField == _currentField)
            randomField = _fieldsToVisit[Random.Range(0, _fieldsToVisit.Length)];
        return randomField;
    }*/
}