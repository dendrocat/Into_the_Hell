EXTERNAL open_shop()

#name:Нарратор
Дверь скрипнула, пропуская Лору в клубы табачного дыма.

#name:Трактирщик
"Чем угощать будем?"

-> drink_choice

=== drink_choice ===
* ["«Крепкое!»"] -> strong_drink
* ["«Поговорить о товарах»"] -> shop_discussion

=== strong_drink ===
#name:Трактирщик
"Держи «Гномий огонь»! *наливает*"
-> lora_reaction

=== shop_discussion ===
#name:Трактирщик
"Ха! Торговать будем? Покажи, на что способна..." 
-> shop_challenge

=== shop_challenge ===
* ["Выпить залпом"] -> respect_ending
* ["Угрожать ножом"] -> shop_ending

=== lora_reaction ===
* ["Выпить залпом"] -> challenge
* ["Изучить зал"] -> observation

=== challenge ===
#name:Лора
*опрокидывает кружку* "Неплохо!"
#name:Трактирщик
"Ты жгёшь!"
-> respect_ending

=== observation ===
#name:Лора
"Осматривает помещение..."
#name:Нарратор
В углу трое спорят о добыче...
-> secret_ending

=== respect_ending ===
#name:Нарратор
"Лору теперь зовут «Горло Железное»!" 
-> END

=== secret_ending ===
#name:Нарратор
"Лора забирает окровавленный нож..." 
-> END

=== shop_ending ===
~ open_shop()
-> END