using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    public float gapX = 0.27f;
    public float gapY = 0.27f;
    public Vector2Int puzzleDim = new Vector2Int(4, 4);
    public Transform poseFist;
    public Transform[] poses;
    //public Sprite[] symbols;
    public GameObject stonePrefab;
    //GameObject[,] stones;

    bool allPiecesPlaced = false;

    [System.Serializable]
    public class Symbols
    {
        public Sprite cristianCross;
        public Sprite daviStar;
        public Sprite om;
        public Sprite ninePointedStar;
        public Sprite moon;
        public Sprite sunCross;
        public Sprite yingYang;
        public Sprite shrine;
        public Sprite wheel;
        public Sprite sikh;
        public Sprite swastika;
        public Sprite hand;
        public Sprite lotus;
        public Sprite triquetra;
        public Sprite celticCross;
        public Sprite slaveCross;
    }
    [SerializeField]
    public Symbols symbols;

    [System.Serializable]
    public class Slot
    {
        public int indexX;
        public int indexY;
        //public Sprite symbol;
        public GameObject stone;
        public Transform pose;

        public Slot(int indexX, int indexY, GameObject stone, Transform pose)
        {
            this.indexX = indexX;
            this.indexY = indexY;
            this.stone = stone;
            this.pose = pose;
        }
    }
    [SerializeField]

    //public List<Piece> initialPieces;
    //public List<Sprite> initialPieces = new List<Sprite>();
    public Sprite[] initialPieces = new Sprite[16];
    Slot[,] Table = new Slot[4, 4];

    //public List<Sprite> InitialSymbols = new List<Sprite>() { };
    //public List<int> ValidInitialPositions = new List<int>();
    //public Dictionary<(int, int), Sprite> InitialSymbols = new Dictionary<(int, int), Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        //int index=0;
        //stones = new GameObject[puzzleDim[0],puzzleDim[1]];
        foreach (Transform pose in poses)
        {
            int index = int.Parse(pose.name.Replace("pos", ""));
            int indexX = int.Parse(pose.name.Replace("pos", "")) / puzzleDim[0];
            int indexY = int.Parse(pose.name.Replace("pos", "")) % puzzleDim[0];
            //print(index);
            print((indexX,indexY));
            //stones[indexX,indexY] = Instantiate(stonePrefab, pose.position, pose.rotation);
            //stones[indexX, indexY].gameObject.GetComponentInChildren<Image>().sprite = symbols.daviStar;
            if (initialPieces[index] != null)
            {
                GameObject stone = Instantiate(stonePrefab, pose.position, stonePrefab.transform.rotation);
                //stone.gameObject.GetComponentInChildren<Image>().sprite = symbols.daviStar;
                stone.GetComponentInChildren<Image>().enabled = true;
                stone.gameObject.GetComponentInChildren<Image>().sprite = initialPieces[index];
                Table[indexX, indexY] = new Slot(indexX, indexY, stone,pose);
            }
            else{
                Table[indexX, indexY] = new Slot(indexX, indexY, null, pose);
            }
            //index++;

            //Piece test = Table[indexX, indexY];
            //if(test.stone != null)
            //    print(test.stone.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (allPiecesPlaced)
        {
            Slot s = CheckIfClicked();
            if (s != null && s.stone != null)
            {
                print(s.stone.name);
                CheckNeighbors(s.indexX, s.indexY, s);
            }
        }
        allPiecesPlaced = CheckAmmountPlaced() >= 15;
    }

    public bool CheckNeighbors(int i, int j,Slot currSlot)
    {
        print((i,j));
        Slot right = null, left = null, upper = null, lower = null;
        if(i+1 < 4)
            lower = Table[i + 1,j];
        if (i-1 > -1)
            upper = Table[i - 1, j];
        if (j + 1 < 4)
            right = Table[i, j + 1];
        if (j - 1 > -1)
            left = Table[i, j - 1];
        if (right != null && right.stone == null)
        {
            print("right");
            //slot.stone.transform.position = right.pose.position;
            //slot.pose = right.pose;
            //right.stone = Instantiate(currSlot.stone, right.pose.position, stonePrefab.transform.rotation);
            right.stone = currSlot.stone;
            right.stone.transform.position = right.pose.position;
            currSlot.stone = null;
            //Destroy(currSlot.stone);
            //(currSlot.indexX, currSlot.indexY) = (right.indexX,right.indexY);
            return true;
        }
        else if (left != null && left.stone == null)
        {
            print("left");
            //left.stone = Instantiate(currSlot.stone, left.pose.position, stonePrefab.transform.rotation);
            left.stone = currSlot.stone;
            left.stone.transform.position = left.pose.position;
            currSlot.stone = null;
            //Destroy(currSlot.stone);
            //(currSlot.indexX, currSlot.indexY) = (left.indexX, left.indexY);
            return true;
        }
        else if (upper != null && upper.stone == null)
        {
            print("up");
            //upper.stone = Instantiate(currSlot.stone, upper.pose.position, stonePrefab.transform.rotation);
            upper.stone = currSlot.stone;
            upper.stone.transform.position = upper.pose.position;
            currSlot.stone = null;
            //Destroy(currSlot.stone);
            //(currSlot.indexX, currSlot.indexY) = (upper.indexX, upper.indexY);
            return true;
        }
        else if (lower != null && lower.stone == null)
        {
            print("down");
            //lower.stone = Instantiate(currSlot.stone, lower.pose.position, stonePrefab.transform.rotation);
            lower.stone = currSlot.stone;
            lower.stone.transform.position = lower.pose.position;
            currSlot.stone = null;
            //Destroy(currSlot.stone);
            //(currSlot.indexX, currSlot.indexY) = (lower.indexX, lower.indexY);
            return true;
        }
        return false;
    }

    public Slot CheckIfClicked()
    {
        for(int i = 0; i<4; i++)
        {
            for (int j=0;j<4;j++)
            {
                Slot p = Table[i,j];
                //print((p.indexX,p.indexY,p.pose));
                if (p.stone != null)
                {
                    if (p.stone.GetComponent<MovablePiece>().clicked)
                    {
                        p.stone.GetComponent<MovablePiece>().clicked = false;
                        return p;
                    }
                }
            }
        }
        return null;
    }

    public int CheckAmmountPlaced()
    {
        int count=0;
        foreach(Slot slot in Table)
        {
            if (slot.stone != null)
                count++;
        }
        return count;
    }
}
