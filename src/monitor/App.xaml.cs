using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace monitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static byte[] huella;

        string s = $@"붿ž뿯⪽뿯玽붿㝁붿灱뿯ᒽ붿┠砮뿯ᢽ붿붿붿ᝰ圕붿뿯㮽뿯熽뿯붿붿뿯徽붿붿뿯붿졼붿뿯ʽ붿붿뿯ᮽ뿯붿붿뿯ઽ뿯붿뿯붿붿붿뿯犽뿯皽崇뿯疽뿯瞽鳫➡뿯ኽṡꗨޒ뿯ઽ뿯붿鷋뿯➽붿붿뿯璽뿯玽붿붿㹧፸䅏뿯ຽ洘橌붿붿䔅佈붿攇眉뇑स幐뿯붿⌊붿붿뿯붿뿯붿뿯ঽ뿯ᦽ뿯붿붿뿯붿뿯箽뿯붿붿뿯붿뿯⾽뿯ᶽ뿯붿睙뿯ઽᬆ跎붿뿯붿ཱ뿯྽곏뿯䊽ᥓ뿯붿ဧ뿯슽붿乃�붿붿핿붿뿯㊽뿯䂽뿯⺽뿯庽뿯붿붿⬆뿯Ჽ뿯붿甼뿯窽뿯붿뿯붿뿯붿뿯纽甑뿯墽뿯⾽㡱뿯岽䑛㵅뿯붿뿯玽붿뿯⾽⼌뿯禽㽁붿뿯䖽뿯綽뿯撽㴗붿붿뿯붿뿯↽뿯暽뿯沽뿯붿붿뿯޽뿯嚽㨡뿯亽붿뿯Ꮍ붿붿붿붿뿯羽붿뿯箽뿯ួ붿椻뿯㦽䅪뿯᾽܆붿㐓뿯㖽賕뿯붿붿场뿯ⶽ붿뿯붿뿯䮽༃뿯㾽漿붿뿯ƽ뿯⪽뿯玽붿㝁붿灱뿯ᒽ붿♟笭뿯᪽붿뿯붿붿붿礿뿯붿붿붿噢붿뿯㦽뿯޽⅝뿯䞽眜㹮≠뿯붿䍁붿붿뿯䦽뿯붿붿붿붿뿯⪽뿯붿䘒籑뿯ઽ뿯붿붿붿�붿붿뿯䂽ች뿯㖽せ⽛뿯붿뿯嚽뿯붿붿洽뿯㦽붿뿯⦽ศ붿뿯榽灑ㄲ붿뿯䢽뿯붿嵓⩋朏䬞붿뿯㾽㩆뿯붿뿯妽붿뿯붿뿯붿뿯ᶽ뿯½뿯붿뿯붿뿯瞽呩뿯붿붿붿뿯纽붿붿《簮氪뿯붿佽붿뿯붿뿯皽뿯஽뿯붿奮瀒뿯粽붿뿯㒽䄿붿붿⌗붿뿯纽붿뿯Ჽ獝뿯⚽孰뿯붿붿붿붿뿯袏붿붿।뿯溽뿯㾽ɦ㉃뿯徽㉹뿯붿붿붿眏붿损ᨐ뿯宽뿯슽붿뿯붿붿붿㍩붿眉뿯붿뿯ẽЏ뿯쾽붿뿯咽᱑뿯붿癟붿붿⁝뿯₽䭋뿯྽뿯붿愉ల붿ꓜ뿯붿⁮ᄝ뿯沽뿯�붿뿯붿ѯ뿯붿뿯�붿帿ཐ뿯璽␼ਡ붿뿯澽붿뿯ƽ뿯⪽뿯玽붿㝁붿붿唔뿯㾽繭堍뿯붿붿붿뿯䖽뿯庽뿯붿뿯붿਀붿䡀ᐱ뿯掽軚䘀뿯ソ䬱裘붿붿붿뿯붿睟杌뿯붿붿붿붿붿2᜚붿붿푣犍଴붿〈ꋍ뿯璽뿯붿硗붿뿯窽٬뿯붿붿붿뿯�碝䠪匪붿붿뿯붿붿뿯붿뿯檽ṁ뿯붿Ȧ붿혾梋ᅛ뿯⢽붿붿뿯熽붿붿뿯붿砂̳丘ᑽ뿯⾽鳛ᩦ붿뿯悽뿯붿桕뿯箽뿯⚽牙뿯䂽뿯྽ဓ㡎뿯붿붿睘ᬢ뿯붿ဎ뿯冽쩩붿⭹뿯ν붿ݽ뿯嶽뿯ⲽ砜뿯宽붿껒뿯붿붿簹뿯붿駈뿯붿붿뿯悽뿯붿붿뿯ソ픓䎜뿯㾽뿯붿붿뿯ࢽ⌍뿯ẽ⬸杄붿뿯檽뿯�㺉붿⬙붿붿뿫쒟붿뿯붿붿붿붿붿爩뿯澽붿붿붿᜿䀂뿯ಽ뿯붿䁅뿯붿絩鿭붿뗝἞뿯䮽붿뿯붿뿯垽쑼➏뿯붿⹜类붿옓붿㡓䘵畁뿯瞽뿯뚷ᕯ漮붿붿붿屳뿯䆽ष뿯熽붿唔뿯㒽뿯⊽뿯枽뿯⦽뿯熽㨞뿯ᚽ붿཈붿뿯붿Ḽ붿뿯箽뿯㾽붿뿯붿ሚ믙붿䰉湱뿯캽붿㐶뿯綽뿯붿붿뿯붿뿯ν뿯䖽〭뿯綽뿯붿뿯亽带뿯䎽⌤뿯붿ㄽ붿뿯皽뿯붿붿믐뿯ᮽ㐶뿯붿붿뿯⎽뿯ᒽ뿯䊽뿯撽뿯嶽뿯ƽ뿯஽뿯䪽붿恅뿯붿ݤ뿯붿붿붿⨀붿㱻뿯붿뿯붿뿯붿藂睄붿뿯傽㐥뿯붿Ԫ뿯咽뿯施℞뿯쒽붿㰋뿯붿뿯ᶽ붿䥯뿯㞽軘붿⤞뿯Ꮍ뿯붿䤯뿯붿뿯㶽浂뿯붿穭붿后⼉堦붿雌붿뿯붿붿䩁㠬怾뿯붿퉿붿붿뿯붿␤붿뿯붿뿯붿뿯㦽뿯ኽ᥃뿯붿붿뿯綽붿夠吲뿯ᦽ뿯ઽ껱뢀뿯亽뿯붿뿯붿뿯䎽붿㍑㱬뿯⾽붿뿯붿尧뿯붿嘤뿯厽伀彌뿯ㆽ붿뿯▽붿幪崆㜰붿㸕뿯붿뿯붿붿㩤뿯붿뿯붿붿뿯붿붿뿯玽⹤뿯붿뿯㢽뿯傽䑳礗뿯ᖽ뿯྽摮뿯붿뿯붿杪o                                      ";
        //private const int MINIMUM_SPLASH_TIME = 2500; // Miliseconds
        //private const int SPLASH_FADE_TIME = 500;     // Miliseconds

        public App()
        {
            System.Text.Encoding encoding = System.Text.Encoding.UTF8; //or some other, but prefer some UTF 
            huella = encoding.GetBytes(s);
        }

        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    // Step 1 - Load the splash screen
        //    SplashScreen splash = new SplashScreen("Assets/splashScreen.png");
        //    splash.Show(false, true);

        //    // Step 2 - Start a stop watch
        //    Stopwatch timer = new Stopwatch();
        //    timer.Start();

        //    // Step 3 - Load your windows but don't show it yet
        //    base.OnStartup(e);
        //    MainWindow main = new MainWindow();

        //    // Step 4 - Make sure that the splash screen lasts at least two seconds
        //    timer.Stop();
        //    int remainingTimeToShowSplash = MINIMUM_SPLASH_TIME - (int)timer.ElapsedMilliseconds;
        //    if (remainingTimeToShowSplash > 0)
        //        Thread.Sleep(remainingTimeToShowSplash);

        //    // Step 5 - show the page
        //    splash.Close(TimeSpan.FromMilliseconds(SPLASH_FADE_TIME));
        //    main.Show();
        //}
    }
}
