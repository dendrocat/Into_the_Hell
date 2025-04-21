EXTERNAL open_upgrade()
EXTERNAL open_consume()

#name:Проныра
Приветствую! Чем могу помочь?
-> choices

=== choices ===
* [Прокачать снаряжение] -> upgrade_ending
* [Купить расходники] -> consume_ending
* [Уйти] -> END

=== upgrade_ending ===
~ open_upgrade()
-> END

=== consume_ending ===
~ open_consume()
-> END