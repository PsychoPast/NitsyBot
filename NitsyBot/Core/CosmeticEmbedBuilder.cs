using Discord;
using Newtonsoft.Json;
using NitsyBot.Core.Setup;
using System.Linq;

namespace NitsyBot.Core
{
    public class CosmeticEmbedBuilder
    {
        private const string CreditFnbr = "https://image.fnbr.co/logo/logo.png";
        private const string FNBRUrl = "https://fnbr.co/api/images?search=";

        public Embed FrenchCosmetic(string item, string type, string icon)
        {
            ApiSetup apireq = new ApiSetup();
            string cos = apireq.Fnbrco(FNBRUrl + item + "&type=" + type);
            dynamic translation = JsonConvert.DeserializeObject(BotSetup.frtranslation);
            dynamic skin = JsonConvert.DeserializeObject(cos);
            string skinid = skin.data[0].id;
            string skinprice = skin.data[0].price;
            string skintype = skin.data[0].readableType;
            string lower = skin.data[0].rarity;
            string skinicon = skin.data[0].images.icon;
            string skinfeat = skin.data[0].images.featured;
            string skinrarity = lower.First().ToString().ToUpper() + lower.Substring(1);
            string skinnamefr;
            string skindescfr;
            try
            {
                skinnamefr = translation[skinid]["name"]["fr"];
                skindescfr = translation[skinid]["description"]["fr"];
                if (skinnamefr == null)
                {
                    skinnamefr = translation[skinid]["name"]["en"];
                }
                if (skindescfr == null)
                {
                    skindescfr = translation[skinid]["name"]["en"];
                }
            }
            catch
            {
                skinnamefr = skin.data[0].name;
                skindescfr = skin.data[0].description;
            }
            string[] ownerinfos = GetOwnerInfos.Infos();
            EmbedBuilder cosmfinal = new EmbedBuilder()
                .WithTitle("**" + skinnamefr + "**")
                .WithThumbnailUrl(CreditFnbr)
                .AddField("Nom:", skinnamefr, false)
                .WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1])
                .AddField("Description:", skindescfr, false)
                .AddField("Prix:", skinprice, false);
            if (icon == "-icon")
            {
                cosmfinal.WithImageUrl(skinicon);
            }
            else if (icon == null || icon != "-icon")
            {
                if (skinfeat != null && !skinfeat.Equals("False"))
                {
                    cosmfinal.WithImageUrl(skinfeat);
                }
                else if (skinfeat == null || skinfeat.Equals("False"))
                {
                    cosmfinal.WithImageUrl(skinicon);
                }
            }
            switch (skinrarity)
            {
                case "Legendary":
                    skinrarity = skinrarity.Replace("Legendary", "Légendaire");
                    cosmfinal.WithColor(Color.LightOrange);
                    break;

                case "Epic":
                    cosmfinal.WithColor(Color.Purple);
                    break;

                case "Uncommon":
                    skinrarity = skinrarity.Replace("Uncommon", "Pas Commun");
                    cosmfinal.WithColor(Color.Green);
                    break;

                case "Rare":
                    cosmfinal.WithColor(Color.Blue);
                    break;

                case "Common":
                    skinrarity = skinrarity.Replace("Common", "Commun");
                    cosmfinal.WithColor(Color.LightGrey);
                    break;

                case "Marvel":
                    cosmfinal.WithColor(Color.DarkRed);
                    break;

                case "Dark":
                    skinrarity = skinrarity.Replace("Dark", "Sombre");
                    cosmfinal.WithColor(Color.DarkPurple);
                    break;

                case "Dc":
                    skinrarity = skinrarity.Replace("Dc", "DC");
                    cosmfinal.WithColor(new Color(0, 0, 0));
                    break;
            }
            cosmfinal.AddField("Rareté:", skinrarity, false);
            switch (skintype)
            {
                case "Outfit":
                    skintype = skintype.Replace("Outfit", "Skin");
                    break;

                case "Pickaxe":
                    skintype = skintype.Replace("Pickaxe", "Pioche");
                    break;

                case "Umbrella":
                    skintype = skintype.Replace("Umbrella", "Parapluie");
                    break;

                case "Glider":
                    skintype = skintype.Replace("Glider", "Planeur");
                    break;

                case "Pet":
                    skintype = skintype.Replace("Pet", "Animal");
                    break;

                case "Skydiving Trail":
                    skintype = skintype.Replace("Skydiving Trail", "Trainée");
                    break;

                case "Emote":
                    skintype = skintype.Replace("Emote", "Danse");
                    break;

                case "Loading Screen":
                    skintype = skintype.Replace("Loading Screen", "Ecran de chargement");
                    break;

                case "Music":
                    skintype = skintype.Replace("Music", "Musique");
                    break;

                case "Toy":
                    skintype = skintype.Replace("Toy", "Objet");
                    break;

                case "Wrap":
                    skintype = skintype.Replace("Wrap", "Revêtement");
                    break;

                case "Banner":
                    skintype = skintype.Replace("Banner", "Bannière");
                    break;

                case "Back Bling":
                    skintype = skintype.Replace("Back Bling", "Sac à dos");
                    break;
            }
            cosmfinal.AddField("Type:", skintype, false);
            return cosmfinal.Build();
        }

        public Embed Cosmetic(string item, string type, string icon)
        {
            ApiSetup apireq = new ApiSetup();
            string cos = apireq.Fnbrco(FNBRUrl + item + "&type=" + type);
            dynamic skin = JsonConvert.DeserializeObject(cos);
            string skinname = skin.data[0].name;
            string skindescription = skin.data[0].description;
            string skinprice = skin.data[0].price;
            string skintype = skin.data[0].readableType;
            string lower = skin.data[0].rarity;
            string skinicon = skin.data[0].images.icon;
            string skinfeat = skin.data[0].images.featured;
            string skinrarity = lower.First().ToString().ToUpper() + lower.Substring(1);
            string[] ownerinfos = GetOwnerInfos.Infos();
            var cosmfinalen = new EmbedBuilder()
                .WithTitle("**" + skinname + "**")
                .WithThumbnailUrl(CreditFnbr)
                .AddField("Name:", skinname, false)
                .WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1])
                .AddField("Description:", skindescription, false)
                .AddField("Rarity:", skinrarity, false)
                .AddField("Price:", skinprice, false)
                .AddField("Type:", skintype);
            if (icon == "-icon")
            {
                cosmfinalen.WithImageUrl(skinicon);
            }
            else if (icon == null || icon != "-icon")
            {
                if (skinfeat != null && !skinfeat.Equals("False"))
                {
                    cosmfinalen.WithImageUrl(skinfeat);
                }
                else if (skinfeat == null || skinfeat.Equals("False"))
                {
                    cosmfinalen.WithImageUrl(skinicon);
                }
            }
            switch (skinrarity)
            {
                case "Legendary":

                    cosmfinalen.WithColor(Color.LightOrange);
                    break;

                case "Epic":

                    cosmfinalen.WithColor(Color.Purple);
                    break;

                case "Uncommon":

                    cosmfinalen.WithColor(Color.Green);
                    break;

                case "Rare":

                    cosmfinalen.WithColor(Color.Blue);
                    break;

                case "Common":

                    cosmfinalen.WithColor(Color.LightGrey);
                    break;

                case "Marvel":

                    cosmfinalen.WithColor(Color.DarkRed);
                    break;

                case "Dark":
                    cosmfinalen.WithColor(Color.DarkPurple);
                    break;

                case "Dc":
                    cosmfinalen.WithColor(new Color(0, 0, 0));
                    break;
            }
            return cosmfinalen.Build();
        }
    }
}