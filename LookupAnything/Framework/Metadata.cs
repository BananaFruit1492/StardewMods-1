﻿using System.Linq;
using Pathoschild.LookupAnything.Framework.Data;
using StardewValley;

namespace Pathoschild.LookupAnything.Framework
{
    /// <summary>Provides metadata that's not available from the game data directly (e.g. because it's buried in the logic).</summary>
    internal class Metadata
    {
        /*********
        ** Accessors
        *********/
        /// <summary>Metadata for game objects (including inventory items, terrain features, crops, trees, and other map objects).</summary>
        public ObjectData[] Objects { get; set; }

        /// <summary>Metadata for NPCs in the game.</summary>
        public CharacterData[] Characters { get; set; }

        /// <summary>Information about Adventure Guild monster-slaying quests.</summary>
        public AdventureGuildQuestData[] AdventureGuildQuests { get; set; }


        /*********
        ** Public methods
        *********/
        /// <summary>Get overrides for a game object.</summary>
        /// <param name="item">The item for which to get overrides.</param>
        /// <param name="context">The context for which to get an override.</param>
        public ObjectData GetObject(Item item, ObjectContext context)
        {
            ObjectSpriteSheet sheet = (item as Object)?.bigCraftable == true ? ObjectSpriteSheet.BigCraftable : ObjectSpriteSheet.Object;
            return this?.Objects
                .FirstOrDefault(obj => obj.SpriteSheet == sheet && obj.SpriteID.Contains(item.parentSheetIndex) && obj.Context.HasFlag(context));
        }

        /// <summary>Get overrides for a game object.</summary>
        /// <param name="character">The character for which to get overrides.</param>
        /// <param name="type">The character type.</param>
        public CharacterData GetCharacter(NPC character, TargetType type)
        {
            string name = character.getName();

            return
                this.Characters?.FirstOrDefault(p => p.ID == $"{type}::{name}") // override by type + name
                ?? this.Characters?.FirstOrDefault(p => p.ID == type.ToString()); // override by type
        }

        /// <summary>Get the adventurer guild quest for the specified monster (if any).</summary>
        /// <param name="monster">The monster name.</param>
        public AdventureGuildQuestData GetAdventurerGuildQuest(string monster)
        {
            return this.AdventureGuildQuests.FirstOrDefault(p => p.Targets.Contains(monster));
        }
    }
}
