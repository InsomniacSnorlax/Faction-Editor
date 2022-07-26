# Faction-Editor

The editor was designed to make faction scriptable objects with relationship data with ease.

![image](https://user-images.githubusercontent.com/94978222/180907036-eec35f50-1d6e-4953-83a3-14736602c7cb.png)


Creating a faction by pressing new faction opens up a window where it will ask for a name and icon(optional) and automatically add the new faction to the relation pool with already existing factions.

![image](https://user-images.githubusercontent.com/94978222/180907165-1f2bdc36-d201-4918-8324-4dcc10cdd3e3.png)


Deleting a faction will work the exact opposite and remove all relations correlating with that faction in the remaining factions. The relationship is controlled by a value between -100 and 100 where it progressively changes the state of the Current Relationship Enum. It was designed in a way that users can use either the enum or the relation value float to determine friend or foe.

Due to its nature of being a scriptable Object, multiple entities can use it and can all be edited at once from the original scriptableObject.
