
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Tetris : MonoBehaviour {

	public GameObject mainCube; 
	public float speed = 10; 
	public Color[] color; 
	
	[Space]
	public Button leftButton;           
	public Button rightButton;        
	public Button leftRotateButton;    
	public Button rightRotateButton;   
	public Button downButton;         
 
	private bool _downButtonIsPressed;  

	private int width = 12; 
	private int height = 20; 

	private GameObject[,] field = new GameObject[0, 0];
	private string[] shapeName = {"O", "L", "J", "T", "S", "Z", "I"}; 
	private GameObject sample, current;
	private int X_field;
	private float timeout, curSpeed;
	private List<GameObject> shape = new List<GameObject>();
	
	private void addPointerDownToButton(Button button, Action action) 
	{
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener((eventData) => action());
		button.gameObject.AddComponent<EventTrigger>().triggers.Add(entry); ;
	}

	void Start()
	{
			initButtons();                  
		sample = new GameObject(); 
		X_field = width/2;
		NewGame();
	}
	private void initButtons()
	{
		addPointerDownToButton(rightButton, () => { if (RightLeft(1)) Move(1); });
		addPointerDownToButton(leftButton, () => { if (RightLeft(-1)) Move(-1); });
		addPointerDownToButton(leftRotateButton, () => Rotation(90));
		addPointerDownToButton(rightRotateButton, () => Rotation(-90));
		
		EventTrigger downEventTrigger = downButton.gameObject.AddComponent<EventTrigger>();
 
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener((eventData) => _downButtonIsPressed = true);
		downEventTrigger.triggers.Add(entry);
 
		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerUp;
		entry.callback.AddListener((eventData) => _downButtonIsPressed = false);
		downEventTrigger.triggers.Add(entry);
 
	}

	void NewGame() 
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			Destroy(transform.GetChild(i).gameObject);
		}
		foreach(GameObject obj in shape)
		{
			Destroy(obj);
		}

		field = new GameObject[width, height];
		CreateShape();

		Debug.Log("Start new game!");
	}

	void CreateCube(Color cubeColor) 
	{
		current = Instantiate(mainCube) as GameObject;
		current.GetComponent<MeshRenderer>().material.color = cubeColor;
		shape.Add(current);
	}
		
	void CreateShape() 
	{
		sample.transform.localEulerAngles = Vector3.zero; 
		shape = new List<GameObject>(); 
		int j = Random.Range(0, shapeName.Length); 

		switch(shapeName[j])
		{
		case "I":
			for(int i = 0; i < 4; i++) 
			{
				CreateCube(color[0]);
				switch(i)
				{
				case 0:
					current.transform.position = new Vector2(X_field, -3);
					break;
				case 1:
					current.transform.position = new Vector2(X_field, -2);
					break;
				case 2:
					current.transform.position = new Vector2(X_field, -1);
					break;
				case 3:
					current.transform.position = new Vector2(X_field, 0);
					break;
				}
			}
			break;

		case "O": 
			CreateCube(color[1]);
			current.transform.position = new Vector2(X_field, 0);
			break;

		case "L":
			for(int i = 0; i < 4; i++)
			{
				CreateCube(color[2]);
				switch(i)
				{
				case 0:
					current.transform.position = new Vector2(X_field, 0);
					break;
				case 1:
					current.transform.position = new Vector2(X_field, -1);
					break;
				case 2:
					current.transform.position = new Vector2(X_field, -2);
					break;
				case 3:
					current.transform.position = new Vector2(X_field + 1, -2);
					break;
				}
			}
			break;

		case "J":
			for(int i = 0; i < 4; i++)
			{
				CreateCube(color[3]);
				switch(i)
				{
				case 0:
					current.transform.position = new Vector2(X_field, 0);
					break;
				case 1:
					current.transform.position = new Vector2(X_field, -1);
					break;
				case 2:
					current.transform.position = new Vector2(X_field,  -2);
					break;
				case 3:
					current.transform.position = new Vector2(X_field - 1,  -2);
					break;
				}
			}
			break;

		case "S":
			for(int i = 0; i < 4; i++)
			{
				CreateCube(color[4]);
				switch(i)
				{
				case 0:
					current.transform.position = new Vector2(X_field, 0);
					break;
				case 1:
					current.transform.position = new Vector2(X_field + 1, 0);
					break;
				case 2:
					current.transform.position = new Vector2(X_field, -1);
					break;
				case 3:
					current.transform.position = new Vector2(X_field - 1, -1);
					break;
				}
			}
			break;

		case "Z":
			for(int i = 0; i < 4; i++)
			{
				CreateCube(color[5]);
				switch(i)
				{
				case 0:
					current.transform.position = new Vector2(X_field, 0);
					break;
				case 1:
					current.transform.position = new Vector2(X_field - 1, 0);
					break;
				case 2:
					current.transform.position = new Vector2(X_field, -1);
					break;
				case 3:
					current.transform.position = new Vector2(X_field + 1, -1);
					break;
				}
			}
			break;

		case "T":
			for(int i = 0; i < 4; i++)
			{
				CreateCube(color[6]);
				switch(i)
				{
				case 0:
					current.transform.position = new Vector2(X_field, 0);
					break;
				case 1:
					current.transform.position = new Vector2(X_field, -1);
					break;
				case 2:
					current.transform.position = new Vector2(X_field - 1, 0);
					break;
				case 3:
					current.transform.position = new Vector2(X_field + 1, 0);
					break;
				}
			}
			break;
		}

		if(GameOver()) 
		{
			Debug.Log("Game over...");	
			NewGame();
		}
	}

	bool GameOver() 
	{
		for(int i = 0; i < shape.Count; i++)
		{
			Transform tr = shape[i].transform;
			int x = Mathf.RoundToInt(tr.position.x);
			int y = Mathf.Abs(Mathf.RoundToInt(tr.position.y));

			if(y < height - 1) 
			{
				if(field[x, y + 1]) 
				{
					return true; 
				}
			}
		}

		return false;
	}

	void AddToField() 
	{
		for(int i = 0; i < shape.Count; i++)
		{
			Transform tr = shape[i].transform;
			int x = Mathf.RoundToInt(tr.position.x); 
			int y = Mathf.Abs(Mathf.RoundToInt(tr.position.y)); 
			field[x, y] = tr.gameObject;
			tr.parent = transform;
		}
	}

	bool Down() 
	{
		for(int i = 0; i < shape.Count; i++)
		{
			Transform tr = shape[i].transform;
			int x = Mathf.RoundToInt(tr.position.x);
			int y = Mathf.Abs(Mathf.RoundToInt(tr.position.y));

			if(y < height - 1) 
			{
				if(field[x, y + 1]) 
				{
					return false; 
				}
			}
			else
			{
				return false; 
			}
		}

		
		foreach(GameObject obj in shape)
		{
			obj.transform.position -= new Vector3(0, 1, 0);	
		}

		return true;
	}

	public void Move(int index) 
	{
		foreach(GameObject obj in shape)
		{
			obj.transform.position += new Vector3(index, 0, 0);
		}
	}

	bool RightLeft(int index) 
	{
		for(int i = 0; i < shape.Count; i++)
		{
			Transform tr = shape[i].transform;
			int x = Mathf.RoundToInt(tr.position.x);
			int y = Mathf.Abs(Mathf.RoundToInt(tr.position.y));

			if(x < width - 1 && index > 0) 
			{
				if(field[x + 1, y]) 
				{
					return false;
				}
			}
			else if(x > 0 && index < 0) 
			{
				if(field[x - 1, y]) 
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		return true;
	}

	bool InsideField() 
	{
		for(int i = 0; i < shape.Count; i++)
		{
			Transform tr = shape[i].transform;
			int x = Mathf.RoundToInt(tr.position.x);
			int y = Mathf.Abs(Mathf.RoundToInt(tr.position.y));

			if(x < 0 || x > width - 1 || y > height - 1 || y < 0)
			{
				return false;
			}
		}

		return true;
	}

	bool CheckOverlap() 
	{
		for(int i = 0; i < shape.Count; i++)
		{
			Transform tr = shape[i].transform;
			int x = Mathf.RoundToInt(tr.position.x);
			int y = Mathf.Abs(Mathf.RoundToInt(tr.position.y));

			if(field[x, y])
			{
				return true;
			}
		}

		return false;
	}

	bool ShiftShape(int iter, int shift) 
	{
		Move(shift); 
		if(InsideField()) 
		{
			if(CheckOverlap()) 
			{
				Move(-shift); 
				return true;
			}
			return false;
		}

		Move(-shift * 2); 
		if(InsideField())
		{
			if(CheckOverlap())
			{
				Move(shift);
				return true;
			}
			return false;
		}
		Move(shift);

		if(iter > 0) 
		{
			return ShiftShape(iter - 1, shift + 1); 
		} 
		else 
		{
			return true;
		}
	}

	void Rotation(int index) 
	{
		bool result = false;
		sample.transform.position = shape[0].transform.position; 

		
		foreach(GameObject obj in shape)
		{
			obj.transform.parent = sample.transform;
		}
		sample.transform.Rotate(0, 0, index);
		foreach(GameObject obj in shape)
		{
			obj.transform.parent = null;
		}

		
		
		if(!InsideField())
		{
			result = ShiftShape(2, 1);
		}
		else 
		{
			result = CheckOverlap();
		}

		
		if(result) 
		{
			foreach(GameObject obj in shape)
			{
				obj.transform.parent = sample.transform;
			}
			sample.transform.Rotate(0, 0, -index);
			foreach(GameObject obj in shape)
			{
				obj.transform.parent = null;
			}
		}
	}

	void FieldUpdate() 
	{
		shape = new List<GameObject>();
		for(int i = 0; i < transform.childCount; i++) 
		{
			shape.Add(transform.GetChild(i).gameObject); 
		}

		int line = -1;
		line = CheckLine();
		while(line != -1)
		{
			DestroyLine(line);
			ShiftLine(line);
			line = CheckLine();
		}
	}

	void ShiftLine(int line) 
	{
		
		for(int x = 0; x < width; x++) 
		{
			for(int y = line; y > 0; y--)
			{
				field[x, y] = field[x, y - 1];
			}
		}

		
		for (int i = 0; i < width; i++) 
		{
			field[i, 0] = null;
		}

		shape = new List<GameObject>();
		for(int i = 0; i < transform.childCount; i++) 
		{
			shape.Add(transform.GetChild(i).gameObject); 
		}

		
		foreach(GameObject obj in shape)
		{
			int y = Mathf.RoundToInt(Mathf.Abs(obj.transform.position.y));
			if(y < line) 
			{
				obj.transform.position -= new Vector3(0, 1, 0);
			}
		}
	}

	void DestroyLine(int line) 
	{
		foreach(GameObject obj in shape)
		{
			int x = Mathf.RoundToInt(obj.transform.position.x);
			int y = Mathf.RoundToInt(Mathf.Abs(obj.transform.position.y));
			if(y == line)
			{
				field[x, y] = null;
				Destroy(obj);
			}
		}
	}

	int CheckLine() 
	{
		int i = 0;
		for(int y = 0; y < height; y++)
		{
			for(int x = 0; x < width; x++)
			{
				if (field[x, y])
					i++;
			}
			if(i == width)
			{
				return y;
			}
			i = 0;
		}
		return -1;
	}



	void Update()
	{
		curSpeed = _downButtonIsPressed ? 0 : speed; // это добавлено
 
		curSpeed = Mathf.Clamp(curSpeed, 0.1f, 1f);
 
		timeout += Time.deltaTime;
		if (timeout > curSpeed)
		{
			timeout = 0;
			if (!Down())
			{
				AddToField();
				FieldUpdate();
				CreateShape();
			}
		}
	}
 
}
