using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    private static int fieldSize = 10;

    private GameObject[,] fieldArray = new GameObject[fieldSize, fieldSize];
    private int[,] shipArray = new int[fieldSize, fieldSize];
    private int[,] shipArrayDes = new int[fieldSize, fieldSize];
    private int[] ships = new int[] {5, 4, 3, 3, 2};
    public Material background;
    public Material playerOne;
    public Material playerTwo;
    private bool missingShip = false;

    public GameObject block;
    public Material notFound;

    private Material found;
    public Text playerOneText;
    public Text playerTwoText;
    public Text destroyedText;
    public Material hitShipP1;
    public Material hitShipP2;

    private int currentPlayer = 2;

    // Use this for initialization
    void Start () {
        restart();
	}

    public void restart()
    {
        int infiniteLoopStop = 100;
        do
        {
            missingShip = false;
            shipArray = new int[fieldSize, fieldSize];
            generateShips(1);
            generateShips(2);
            fieldInit();
            --infiniteLoopStop;
        } while (missingShip && infiniteLoopStop >= 0);
        if (infiniteLoopStop < 0)
        {
            Debug.Log("Couldnt create Game");
        }
        shipArrayDes = (int[,])shipArray.Clone();
        currentPlayer = 2;
        playerOneText.text = "-->Spieler 1";
        playerTwoText.text = "Spieler 2";
    }

    void fieldInit()
    {
        for (int i = 0; i < fieldArray.GetLength(1); ++i)
        {
            for (int e = 0; e < fieldArray.GetLength(0); ++e)
            {
                GameObject cube = fieldArray[i, e];
                if (cube == null)
                {
                    cube = Instantiate(block, new Vector3(i, e, 0), Quaternion.identity);
                }
                if (shipArray[i, e] == 0)
                {
                    cube.GetComponent<Renderer>().material = background;
                } else if (shipArray[i,e] == 1)
                {
                    cube.GetComponent<Renderer>().material = playerOne;
                }
                else
                {
                    cube.GetComponent<Renderer>().material = playerTwo;
                }
                cube.transform.position = new Vector3(i ,e ,0);
                fieldArray[i, e] = cube;
            }
        }
    }

    bool xHelper(int x, int y, int faktor, int shipLength, int player)
    {
        if (x + faktor*shipLength < shipArray.GetLength(0) && x + faktor * shipLength > 0)
        {

            int start = faktor < 0 ? x - shipLength + 1 : x;
            if (start > 0 && shipArray[start - 1, y] == player)
            {
                return false;
            }
            if (start + shipLength < shipArray.GetLength(0) && shipArray[start + shipLength, y] == player)
            {
                return false;
            }
            for (int i = start; i < start + shipLength; ++i)
            {
                if(shipArray[i,y] != 0)
                {
                    return false;
                }
                if (y > 0 && shipArray[i,y-1] == player)
                {
                    return false;
                }
                if(y < shipArray.GetLength(1) -1 && shipArray[i, y + 1] == player)
                {
                    return  false;
                }
            }
            for (int i = start; i < start+shipLength; ++i)
            {
                shipArray[i, y] = player;
            }
            return true;
        }
        return false;
    }


    bool yHelper(int x, int y, int faktor, int shipLength, int player)
    {
        if (y + faktor * shipLength < shipArray.GetLength(1) && y + faktor * shipLength > 0)
        {
            int start = faktor < 0 ? y - shipLength + 1 : y; //5,3 ::: 
            if (start > 0 && shipArray[x, start - 1] == player) //Ist da ein Befreundetes Schiff am Ende? 
            {
                return false;
            }
            if (start + shipLength< shipArray.GetLength(1) && shipArray[x, start + shipLength] == player) //Ist am Anfang des Shiffes ein befreundetes?
            {
                return false;
            }
            for (int i = start; i < start + shipLength; ++i)
            {
                if (shipArray[x, i] != 0) //Aktuelle Position hat schon wer anders?
                {
                    return false;
                }
                if (x > 0 && shipArray[x - 1, i] == player) // liegt überhalb ein befreundetes Schiff?
                {
                    return false;
                }
                if (x < shipArray.GetLength(0) -1 && shipArray[x + 1, i] == player) //liegt unterhalb ein befreundetes Schiff ?
                {
                    return false;
                }
            }
            for (int i = start; i < start+shipLength; ++i)
            {
                shipArray[x, i] = player;
            }
            return true;
        }
        return false;
    }

    void generateShips(int player)
    {
        for (int i = 0; i < ships.GetLength(0); ++i)
        {
            int retrys = 10;
            bool found = false;
            while (retrys >= 0 && !found) {
                int x = Random.Range(0, 10);
                int y = Random.Range(0, 10);
                int direction = Random.Range(0, 4);

                switch (direction)
                {
                    case 0:
                        found = yHelper(x, y, 1, ships[i], player);
                        break;
                    case 1:
                        found = yHelper(x, y, -1, ships[i], player);
                        break;
                    case 2:
                        found = xHelper(x, y, 1, ships[i], player);
                        break;
                    case 3:
                        found = xHelper(x, y, -1, ships[i], player);
                        break;
                }
                retrys--;
            }
            if(retrys < 0)
            {
                missingShip = true;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}



    private void FixedUpdate()
    {
        Touch touch = Input.touches[0];
        if(touch.phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
                Debug.Log(hit.transform.gameObject.name);
                for (int i = 0; i < fieldArray.GetLength(1); ++i)
                {
                    for (int e = 0; e < fieldArray.GetLength(0); ++e)
                    {
                        if (fieldArray[i, e] == hit.transform.gameObject)
                        {
                            if (shipArray[i, e] == 0)
                            {
                                notFound.color = background.color;
                                hit.transform.gameObject.GetComponent<Renderer>().material = notFound;
                                hit.transform.gameObject.transform.position += new Vector3(0,0,-1);
                            }
                            else if(shipArray[i,e] != currentPlayer)
                            {
                                shipArrayDes[i, e] = currentPlayer == 1 ? 3 : 4;
                                found = currentPlayer == 1?hitShipP1:hitShipP2;
                                hit.transform.gameObject.GetComponent<Renderer>().material = found;
                                detectDestroyedShips();
                                hit.transform.gameObject.transform.position += new Vector3(0, 0, -1);
                                continue;
                            }
                            else
                            {
                                continue;
                            }
                            currentPlayer = currentPlayer == 1 ? 2 : 1;
                            playerOneText.text = currentPlayer == 2 ? "-->Spieler 1" : "Spieler 1";
                            playerTwoText.text = currentPlayer == 1 ? "-->Spieler 2" : "Spieler 2";
                        }
                    }
                }
            }
        }
    }

    private void detectDestroyedShips()
    {
        int countedPlayerOneShipParts = 0;
        int countedPlayerTwoShipParts = 0;
        for(int i = 0; i < shipArrayDes.GetLength(0); ++i){
            for(int e = 0; e < shipArrayDes.GetLength(1); ++e)
            {
                if(shipArrayDes[i,e] == 2)
                {
                    ++countedPlayerOneShipParts;
                }else if (shipArrayDes[i,e] == 1)
                {
                    ++countedPlayerTwoShipParts;
                }
            }
        }
        //destroyedText.text = "Schiffe Spieler 1:"+countedPlayerOneShipParts+" Schiffe Spieler 2:"+countedPlayerTwoShipParts ;
        if (countedPlayerOneShipParts == 0)
        {
            destroyedText.text = "Spieler 2 gewinnt";
        }
        if(countedPlayerTwoShipParts == 0)
        {
            destroyedText.text = "Spieler 1 gewinnt";
        }
    }

}
