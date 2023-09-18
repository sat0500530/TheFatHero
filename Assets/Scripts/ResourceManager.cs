using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using JetBrains.Annotations;
using Mono.Cecil;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceManager : MonoBehaviour
{
    public List<FieldEventInfo> FieldEvents { get; private set; }

    public List<ItemInfo> Items { get; private set; }
    
    public List<Artifact> Artifacts { get; private set; }

    public Sprite healEventSprite;
    
    public void Start()
    {
        InitFieldEvent();
        InitMonster();
        InitItemEvent();
        InitArtifact();

        // LoadData();
    }

    // public void LoadData()
    // {
    //     string[] data;
    //     TextAsset field = Resources.Load("FieldEvent/events") as TextAsset;
    //     data = field.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
    //
    //     FieldEvents = new();
    //     for (int i = 0, cnt = data.Length / 2; i < cnt ; i++)
    //     {
    //         string text = data[2 + (i * 2)];
    //         Sprite sprite = Resources.Load($"{data[3 * (i + 1) + 1]}").GetComponent<Sprite>();
    //     
    //         var fieldInfo = new FieldEventInfo(sprite, text); 
    //         FieldEvents.Add(fieldInfo);
    //     }
    //
    //     
    // }

    void InitFieldEvent()
    {
        FieldEvents = new();
        FieldEvents.Add(new(EventType.HP, 1, GetSrc("FieldEvent", "blessinglake"), "축복의 샘입니다.\n\n체력이 1 회복됩니다."));
        FieldEvents.Add(new(EventType.HP, 2, GetSrc("FieldEvent", "fruitevent"), "수풀을 헤치며 지나가고 있었습니다. 기다렸다는 듯, 탐스러워 보이는 열매를 찾았습니다./아직 신은 나를 버리지 않았나 봅니다. \n\n체력이 2 회복됩니다."));
        FieldEvents.Add(new(EventType.Power, 1, GetSrc("FieldEvent", "boyevent"), "늑대들에게 둘러싸여 있는 한 소년을 발견하고, 검을 뽑고 달려가 늑대들을 물리쳤습니다./소년은 감사하다는 인사를 하며, 자기도 꼭 커서 용사가 될 것이라 다짐합니다. 흐뭇한 표정을 지으며 갈 길을 이어서 갑니다. \n파워가 1 올라갑니다."));
        FieldEvents.Add(new(EventType.Dex, 3, GetSrc("FieldEvent", "ghost"), "갑자기 등골이 오싹해 집니다. 뒤를 돌아보니 귀신이 저를 쳐다보고 있습니다./화들짝 놀라 전력질주 합니다. \n\n 민첩이 3 증가합니다."));
        FieldEvents.Add(new(EventType.HP, -1, GetSrc("FieldEvent", "blood"), "서둘러 나아가다, 바닥에 고인 핏물에 미끄러집니다. \n\n체력이 1 줄어듭니다."));
        FieldEvents.Add(new(EventType.HP, -2, GetSrc("FieldEvent", "goblinevent"), "수풀을 헤치며 지나가고 있었습니다. 갑자기 주위에서 고블린 덮칩니다./힘겹게 급습을 막아냈으나, 너무나 지칩니다.\n\n체력이 2 줄어듭니다."));
        FieldEvents.Add(new(EventType.StateCount, 3, GetSrc("FieldEvent", "cross"), "저 멀리 십자가가 보입니다.\n기사는 다시금 마음을 굳힙니다./성스러워진 마음은 몸을 강하게 해줍니다. \n\n질병이 모두 없어집니다!"));
        FieldEvents.Add(new(EventType.Hunger, -10, GetSrc("FieldEvent", "goblinfood"), "저기 고기를 먹는 고블린들이 보입니다.\n노릇노릇한 향기가 코 끝을 스칩니다./더 배가 고파집니다. \n\n포만감이 10 줄어듭니다.."));
        FieldEvents.Add(new(EventType.Hunger, 10, GetSrc("FieldEvent", "floorfood"), "앗 바닥에 고기가 놓여져있습니다.\n고민 따위 하지 않았습니다./입에 우겨넣었습니다. \n\n포만감이 10 회복됩니다."));
        FieldEvents.Add(new(EventType.Cold, 0, GetSrc("FieldEvent", "getcold"), "성 안이 급격하게 추워집니다./몸이 으슬으슬 합니다. \n\n감기에 걸립니다."));
        FieldEvents.Add(new(EventType.HP, -2, GetSrc("FieldEvent", "arrow"), "'딸깍'\n불길한 예감이 듭니다./사방에서 화살이 날라옵니다. 빠르게 피했지만, 모두 피할 순 없었습니다. \n\n체력이 2 줄어듭니다."));
        FieldEvents.Add(new(EventType.Cost, 2, GetSrc("FieldEvent", "shortcut"), "잠시 지쳐 벽에 기댔는데, 무언가 눌리는 느낌이 났습니다.\n큰 소리와 함께 지름길이 등장했습니다./행동력이 2 회복됩니다."));
        FieldEvents.Add(new(EventType.Cost, 1, GetSrc("FieldEvent", "wind"), "창문으로 시원한 바람이 불어옵니다.\n시원한 바람은 발걸음을 가볍게 해줍니다./행동력이 1 회복됩니다."));
    }

    public void InitArtifact()
    {
        var data = DataManager.Instance;
        Artifacts = new List<Artifact>();
        Artifacts.Add(new Artifact(ArtifactType.AllStatUp, "공주의 편지", $"전체 스텟이 {data.ARTI_AllStatUp_Value}만큼 증가합니다.", GetSrc("Artifact", "princessletter")));
        Artifacts.Add(new Artifact(ArtifactType.AddAttack, "분신술 비급서", $"용사는 {data.ARTI_AddAtack_Interval}공수마다 한번 더 공격합니다.", GetSrc("Artifact", "ninja")));
        Artifacts.Add(new Artifact(ArtifactType.DexUp, "날개 달린 신발", $"용사의 민첩이 {data.ARTI_DEXUP_Value}만큼 증가합니다.", GetSrc("Artifact", "flyingshoes")));
        Artifacts.Add(new Artifact(ArtifactType.CostUp, "반짝이는 돌", $"기이한 돌의 효과로 용사와 공주는 {data.ARTI_COSTUP_Value}만큼 추가 행동력을 가집니다.", GetSrc("Artifact", "shinestone")));
        Artifacts.Add(new Artifact(ArtifactType.PrincessSkillUp, "유니콘의 뿔", $"공주의 성력이 증가하여, 용사 강화 시 {data.ARTI_PrincessSkillUP_Value}만큼 추가 강화합니다.", GetSrc("Artifact", "unicorn")));
        Artifacts.Add(new Artifact(ArtifactType.KnightSkillUp, "빠짝 마른 장작", $"휴식 스킬 사용 시 {data.ARTI_KnightSkillUP_Value}만큼 추가 휴식합니다.", GetSrc("Artifact", "tree")));
        Artifacts.Add(new Artifact(ArtifactType.HpUp, "트롤의 피", $"체력이 {data.ARTI_HPUP_Value}만큼 증가합니다.", GetSrc("Artifact", "blood")));
    }



    public Monster Ruggle;
    public Monster DeathKnight;
    public Monster Dragon;


    void InitMonster()
    {
        Ruggle = new Monster("러글", new(10, 5, 10, 5), GetSrc("Monster", "ruggle"));
        DeathKnight = new Monster("데스나이트", new(20, 7, 15, 10), GetSrc("Monster", "deathknight"));
        Dragon = new Monster("드래곤", new(40, 10, 20, 30), GetSrc("Monster", "dragon"));
    }

    void InitItemEvent()
    {
        Items = new();
        Items.Add(new(EventType.Hunger, 15, GetSrc("ItemEvent", "fruit"), "고기를 주웠다! \n\n포만감을 15 회복됩니다..!"));
    }

    Sprite GetSrc(string folder, string name)
    {
        return Resources.Load<Sprite>($"{folder}/{name}");
    }

    public Monster GetRandomMonster(int monlevel)
    {
        int[] levelRanges = {
        1, 3, 5, 8, 10, 13, 16, 20, 23, 25, 28, 31, 34, 37, 39
        };

        int index = -1;

        // Find the appropriate index for the given monlevel
        for (int i = 0; i < levelRanges.Length; i++)
        {
            if (monlevel <= i + 1)
            {
                index = Random.Range(i == 0 ? 0 : levelRanges[i - 1], levelRanges[i]);
                break;
            }
        }

        return index switch
        {
            0 => new Monster("Lv.1 <color=#FF0000>슬라임</color>", new(3, 1, 1, 1), GetSrc("Monster", "slime")),
            //******************************************↑Level1↑******************************************
            1 => new Monster("Lv.2 <color=#FF0000>고블린</color>", new(3, 2, 3, 2), GetSrc("Monster", "goblin")),
            2 => new Monster("Lv.2 <color=#FF0000>슬라임</color>", new(4, 1, 3, 2), GetSrc("Monster", "slime")),
            //******************************************↑Level2↑******************************************
            3 => new Monster("Lv.3 <color=#FF0000>고블린</color>", new(4, 2, 5, 3), GetSrc("Monster", "goblin")),
            4 => new Monster("Lv.3 <color=#FF0000>슬라임</color>", new(5, 1, 5, 3), GetSrc("Monster", "slime")),
            //******************************************↑Level3↑******************************************
            5 => new Monster("Lv.4 <color=#FF0000>고블린</color>", new(4, 3, 14, 4), GetSrc("Monster", "goblin")),
            6 => new Monster("Lv.4 <color=#FF0000>슬라임</color>", new(5, 2, 14, 4), GetSrc("Monster", "slime")),
            7 => new Monster("Lv.4 <color=#FF0000>오크</color>", new(5, 4, 0, 4), GetSrc("Monster", "orc")),
            //******************************************↑Level4↑******************************************
            8 => new Monster("Lv.5 <color=#FF0000>고블린</color>", new(7, 3, 14, 5), GetSrc("Monster", "goblin")),
            9 => new Monster("Lv.5 <color=#FF0000>오크</color>", new(8, 4, 0, 5), GetSrc("Monster", "orc")),
            //******************************************↑Level5↑******************************************
            10 => new Monster("Lv.6 <color=#FF0000>고블린</color>", new(9, 3, 14, 6), GetSrc("Monster", "goblin")),
            11 => new Monster("Lv.6 <color=#FF0000>오크</color>", new(10, 4, 0, 6), GetSrc("Monster", "orc")),
            12 => new Monster("Lv.6 <color=#FF0000>스텀프</color>", new(10, 3, 14, 6), GetSrc("Monster", "stump")),
            //******************************************↑Level6↑******************************************
            13 => new Monster("Lv.7 <color=#FF0000>고블린</color>", new(10, 3, 14, 7), GetSrc("Monster", "goblin")),
            14 => new Monster("Lv.7 <color=#FF0000>오크</color>", new(11, 4, 0, 7), GetSrc("Monster", "orc")),
            15 => new Monster("Lv.7 <color=#FF0000>스텀프</color>", new(11, 3, 19, 7), GetSrc("Monster", "stump")),
            //******************************************↑Level7↑******************************************
            16 => new Monster("Lv.8 <color=#FF0000>고블린</color>", new(11, 3, 14, 8), GetSrc("Monster", "goblin")),
            17 => new Monster("Lv.8 <color=#FF0000>오크</color>", new(11, 4, 0, 8), GetSrc("Monster", "orc")),
            18 => new Monster("Lv.8 <color=#FF0000>스텀프</color>", new(12, 3, 19, 8), GetSrc("Monster", "stump")),
            19 => new Monster("Lv.8 <color=#FF0000>드레이크</color>", new(13, 3, 19, 8), GetSrc("Monster", "drake")),
            //******************************************↑Level8↑******************************************
            20 => new Monster("Lv.9 <color=#FF0000>오크</color>", new(13, 4, 0, 9), GetSrc("Monster", "orc")),
            21 => new Monster("Lv.9 <color=#FF0000>스텀프</color>", new(13, 3, 19, 9), GetSrc("Monster", "stump")),
            22 => new Monster("Lv.9 <color=#FF0000>드레이크</color>", new(13, 3, 19, 9), GetSrc("Monster", "drake")),
            //******************************************↑Level9↑******************************************
            23 => new Monster("Lv.10 <color=#FF0000>스텀프</color>", new(16, 4, 19, 10), GetSrc("Monster", "stump")),
            24 => new Monster("Lv.10 <color=#FF0000>드레이크</color>", new(17, 4, 19, 10), GetSrc("Monster", "drake")),
            //******************************************↑Level10↑******************************************
            25 => new Monster("Lv.11 <color=#FF0000>스텀프</color>", new(18, 4, 19, 11), GetSrc("Monster", "stump")),
            26 => new Monster("Lv.11 <color=#FF0000>드레이크</color>", new(18, 4, 19, 11), GetSrc("Monster", "drake")),
            27 => new Monster("Lv.11 <color=#FF0000>토글</color>", new(1, 2, 50, 11), GetSrc("Monster", "togle")),
            //******************************************↑Level11↑******************************************
            28 => new Monster("Lv.12 <color=#FF0000>스텀프</color>", new(19, 4, 21, 12), GetSrc("Monster", "stump")),
            29 => new Monster("Lv.12 <color=#FF0000>드레이크</color>", new(18, 4, 21, 12), GetSrc("Monster", "drake")),
            30 => new Monster("Lv.12 <color=#FF0000>토글</color>", new(1, 3, 50, 12), GetSrc("Monster", "togle")),
            //******************************************↑Level12↑******************************************
            31 => new Monster("Lv.13 <color=#FF0000>드레이크</color>", new(20, 4, 24, 13), GetSrc("Monster", "drake")),
            32 => new Monster("Lv.13 <color=#FF0000>토글</color>", new(1, 3, 55, 13), GetSrc("Monster", "togle")),
            33 => new Monster("Lv.13 <color=#FF0000>방패병</color>", new(19, 4, 0, 13), GetSrc("Monster", "shield")),
            //******************************************↑Level13↑******************************************
            34 => new Monster("Lv.14 <color=#FF0000>드레이크</color>", new(21, 4, 29, 14), GetSrc("Monster", "drake")),
            35 => new Monster("Lv.14 <color=#FF0000>토글</color>", new(1, 3, 60, 14), GetSrc("Monster", "togle")),
            36 => new Monster("Lv.14 <color=#FF0000>방패병</color>", new(21, 4, 0, 14), GetSrc("Monster", "shield")),
            //******************************************↑Level14↑******************************************
            37 => new Monster("Lv.15 <color=#FF0000>토글</color>", new(1, 4, 75, 15), GetSrc("Monster", "togle")),
            38 => new Monster("Lv.15 <color=#FF0000>방패병</color>", new(22, 5, 0, 15), GetSrc("Monster", "shield")),
            //******************************************↑Level15↑******************************************



            _ => null,
        };

    }

    public FieldEventInfo GetRandomFieldEvent(int floor)
    {
        int index;

        if (floor == 0)
        {
            index = Random.Range(0, FieldEvents.Count);
        }

        else if (floor == 1)
        {
            index = Random.Range(0, FieldEvents.Count);
        }

        else
        {
            index = Random.Range(0, FieldEvents.Count);
        }

        return FieldEvents[index];
    }

    public ItemInfo GetRandomItemEvent()
    {
        int index = Random.Range(0, Items.Count);
        return Items[index];
    }
}


public enum EventType
{
    HP,
    Power,
    Dex,
    Exp,
    Hunger,
    StateCount,
    Cold,
    Cost

}