using Core;

namespace Model
{
using System.Collections.Generic;


public class Equip
{
    public int Id;
    public int Level;
    public int Exp;
    public string Name;
}
public class Player
{
    public  long   Uid;
    public  string Name;
    private int    level;

    public int Level
    {
        get
        {
            return level;
        } 
        set
        {
            level = value;   
        }
    }

    public Equip       Weapon;
    public List<Equip> EquipList;
    public Equip[]     EquipArray;
    public Dictionary<int, Equip> EquipDict;

    public Player()
    {
        // Equips = new List<Equip>();
    }
    
    public Player DeepClone()
    {
        Player player = MemberClone();
        player.Weapon = new Equip();
        player.Weapon.Id = Weapon.Id;
        player.Weapon.Level = Weapon.Level;
        player.Weapon.Exp = Weapon.Exp;
        player.Weapon.Name = Weapon.Name;
        player.EquipList = new List<Equip>(){player.Weapon};
        player.EquipArray = new Equip[1]{player.Weapon};
        player.EquipDict = new Dictionary<int, Equip>{{player.Weapon.Id, player.Weapon}};

        return player;
    }
    
    public Player MemberClone()
    {
        Log.Warning("MemberClone");
        return (Player)MemberwiseClone();
    }

}

}