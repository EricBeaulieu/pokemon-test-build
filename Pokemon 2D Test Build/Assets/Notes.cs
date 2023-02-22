using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes
{
    //Alternative types of starter Pokemon
    //Rock Fighting and flying
    //Fighting psychic and dark. problem with that is dark would have immunity over it

    //To do list
    //update Attack button to inherit from DyanmicButton, also update the color to be a gradient rather then a function
    //Update health bar to be a gradient as well rather than a function
    //Fix the vendors talking distance, if i talked from underneath it doesnt work but from the right it does
    //create player and enemies team lineup on battle
    //with a wild battle, if both pokemon faint show the animation for both pokemon dying, currently it just ends with the players unit still standing even though they wont get the exp
    //fix the smooth transistion calls in battle unit
    //fix the preanimtion for confused

    //Shop
    //Noticed when you changed the amount to buy, the item selection flashed and caught my eye.
    //repel/super/max repel and pokedoll for shop

    //iron ball for grounded
    //fix stealth rock to not miss when target is semi invernerable
    //Update catching mechanics once happiness has been added and implimented

    //power points stuff not implimented for when item is used on pokemon
    //in regards to specialized messages, they can be thrown into stat boost and displayed when given into effect
    //substitute, when made add in infiltrator
    //Change the way the pokemon gets its nature
    //Cheek pouch ability,Gluttony,Harvest for when berries are implimented
    //Comatose, Corrosion,dancer , majority of complicated abilities for later
    //bullet proof fix the list to contain moves that it shall adjust
    //fix the save for the player. currently after every save the player seems to be stuck running
    //when a player looses a battle, reset everything, the pokemon trainers were still standing in the positions that they last were
    //Battle unit, remove the event from pokemon and put it over to battle unit and have it update the HUD upon so, change in battlesystem the set status
    //alter xp modifier if they can evolve

    //add in specific moves
    //transform
    //smack down + Thousand Arrows -- needs a special change in the charging turn class
    //variations of arena trap
    //oblivious taunts
    //Embargo implimented but needs to be put into battle system
    //Encore specialized move failures need to be implimented
    //anticipation for one hit ko moves
    //one hit ko moves

    //punk rock sounds reduction based
    //normalize to change the type of status moves as well

    //roost creating it so the pokemon can get hit by ground moves
    //finish all different types of animations with conditions
    //camera effects when entering battle either with trainers,wild pokemon or specialized pokemon
    //When level manager has overowlrd weather effects to pass them into the battles

    //Add in

    //Roar
    //aura break when fairy aura and dark aura are added in, all these abilities share the info together

    //Wishes
    //Overworld effects with abilitities
    //Flame Burst when double battles entered in, This move does damage to the Pokémon to the side of the target equal to 1/16th of their maximum Hit Points
    //pollen puff to heal ally

    //Bugs
    //on turn end for bound moves the pokemon doesnt play flash of hit animation

    //Others
    //set/Shift option, if set just keep pokemon out

    /* Items
     * 
     * list of enum items that are added but not implimented
     * CleanseTag,DestinyKnot,EjectButton, EjectPack,PowerHerb,RedCard,Ring Target,RoomService,SootheBell,UtilityUmbrella,ZoomLens,'
     */

    /*attacks not implimented 
    beak blast, beat up, bide,Bug Bite,Circle Throw,dragon tail, fling,flying press,Fusion Bolt, Heat Crash,Heavy Slam,
    Hyperspace Fury,Ice Ball,Low Kick,Metal Burst,Multi-Attack,Natural Gift,Outrage,Pay Day,Pluck,Present,Pursuit,Rapid Spin,
    Retaliate,Rollout,Secret Power,Sky Drop,Smack Down,Spectral Thief,Sunsteel Strike,Thousand Arrows,Thrash,U-turn

    Belch, Burn Up,Core Enforcer,Doom Desire,Dragon Rage,Final Gambit,Fire/Grass/Water Pledge,
    Fusion Flare,Future Sight,Grass Knot,Judgment,Mirror Coat,Moongeist Beam,Nature's Madness,Night Shade,Petal Dance,
    Photon Geyser,Psyshock,Psystrike,Relic Song,Revelation Dance,Secret Sword,Sheer Cold,Sonic Boom,Spit Ups,Techno Blast,Volt Switch

    After You, Ally Switch,Aromatherapy,Aromatic Mist,Assist,Baneful Bunker,Baton Pass,Bestow,Camouflage,Captivate
    */

    //Incinerate berry removal
    //gust/twister double damage against flying unit(semi-invernerable state)
    //Freeze-Dry and flying press have their own specialzed type manor
    //Uproar pokemon cant fall asleep during these turns


    //Additions that will need to be mentioned
    //earth power now hits pokemon while theyre semi invernerable while theyre dug underground
    //magnitude and earth power doubles and fissure accuracy increases to 50% while theyre in dig state
    //Hidden Power Attack Power increased to 90, also includes fairy type



    /*In regards to taking pictures, a screen shot tab has been added, in order to utilize this. the current scene must be open, 
    it will grab everything that the current camera can see. the size is 16.28 that all photos were taken in.
    Normally the camera will be set to 6 to keep the players bounds box small
    */
}
