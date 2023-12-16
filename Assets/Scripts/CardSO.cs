using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardSO/new card")]
public class CardSO : ScriptableObject
{
    public enum Name {acorn, amanita, apple , asparagus, avocado, 
        bacon, bananas,beer,beet, blueberry,bobs,bone,borth,
        bread,bread2,bread3,brocoli,butter,cabbage,cake2,cake3,
        candy,candy2,carrot,celery,champignon,cheese,cheese2,
        cheesecake,cherry,chiken,chocolate,cocount, coffee,
        cookies, corn,croissant,cucumber,cupcake,dill,donuts,eggplant,
        eggs,fish,fishSkeleton,fishSteak,fish2,garlic,granat,grapes,grapes1,
        ham,honey,ice, iceCream,jelly, kiwi,legChicken,lemon,mandrakeRoot,
        meat,melone,milk,mint,mushroom,nut,octopus,oil,onion,orange,pasties,
        peach,pear,peas,pie,pineapple,porridge,potato,pumpiking,radish,
        radish2,redPepper,salat,sausage,schnitzel,seaBuckthorn,shell,shrimp,
        spice1,spice2,squid,straberry,sunflowerSeeds,tea,tomato,vine,water,
        watermelon,wheat,yellowPepper}
    [SerializeField]Sprite sprite;
    [SerializeField] Name cardName;
    public Name GetName() => cardName;
    public void SetName(Name _name) => cardName = _name;
    public Sprite GetSprite() => sprite;
    public void SetSprite(Sprite _sprite) => sprite = _sprite;

}
