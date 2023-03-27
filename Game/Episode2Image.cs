﻿/*
Образ эпизода 2.

2023-01-04
*/

namespace TestGame
{
    /// <summary>
    /// Образ эпизода 2.
    /// </summary>
    internal class Episode2Image : SceneImage
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Episode2Image()
        {
            Items = new byte[,]{
/*
0*  TerrainLayer
//00  песок с вкраплениями дорожек
//01  тропа на песке  =
//02  тропа на песке ||
//03  болото
04  озеро
//05  шоссе левая полоса
//06  шоссе средняя полоса
//07  шоссе правая полоса
//08  брод
09  каменная поверхность
10  тропа на камне =
11  тропа на камне ||
12  пропасть
13  скалистый перешеек =
14  скалистый перешеек ||

2*  InteractionLayer
20  скала
21  темная скала
22  -
//23  дерево
24  мост =
25  мост ||
//26  гора
//27  вход в пещеру ->
28  дверь правая
29  дверь верхняя
30  ящик
31  бочка
32  бочка пустая
33  крепь
34  куча
35  костер

36  скриптованный объект 1
37  скриптованный объект 2
38  скриптованный объект 3
39  скриптованный объект 4

//4*  Забор
//40  None = 0,       45 43 47
//41  Left = 1,       41 40 42
//42  Right = 2,      46 44 48
//43  Up = 3,
//44  Down = 4,
//45  LeftUp = 5,
//46  LeftDown = 6,
//47  RightUp = 7,
//48  RightDown = 8
49  Мишень
50  Флаг

6*  NPC
60  Guardian
61  Chaser
62  Shooter
//63  Cow
64  Медведь
65  Анна
66  Снайпер

7*  Автомобили
//70  Седан, вправо, перед     70,71,72
//71  Седан, вправо, центр     73,74,75
//72  Седан, вправо, зад
73  Пикап, вправо, перед
74  Пикап, вправо, центр 
75  Пикап, вправо, зад

8 

9*  Трофеи
90  Аптечка
91  Патроны
92  Огнестрел
*/

{20,20,20,20,20,20,21,21,20,20,20,21,21,21,21,20, 21,21,20,20,21,21,33,21,20,21,21,33,04,04,21,21, 04,21,04,21,33,33,21,21,21,20,20,20,20,21,21,20},
{20,21,21,21,21,21,33,33,21,21,21,00,00,00,10,14, 10,00,20,20,00,10,10,66,14,10,10,10,24,24,10,10, 24,14,24,10,10,00,00,35,00,21,20,20,21,33,33,20},
{20,31,11,11,00,00,11,00,11,00,00,11,11,00,00,20, 00,11,20,21,10,33,33,20,20,00,35,66,04,04,04,04, 04,00,04,04,00,10,10,10,10,00,20,21,00,10,00,20},
{20,30,11,35,11,00,11,38,11,00,11,35,11,00,12,21, 20,11,14,28,10,00,00,21,20,00,04,04,04,04,04,04, 04,04,04,04,04,00,20,20,20,13,20,00,11,20,11,20},
{20,32,11,11,11,00,11,11,11,00,11,11,11,00,00,12, 20,13,20,20,20,90,20,00,00,00,04,04,04,04,00,04, 04,04,04,04,04,00,21,20,21,11,20,00,11,21,11,20},
{20,20,11,35,11,00,61,00,11,00,11,35,11,20,20,12, 21,11,21,21,20,20,21,00,04,04,04,04,04,04,04,04, 04,04,04,04,04,04,04,20,33,11,21,11,20,13,13,20},
{20,20,11,11,11,00,00,00,11,00,11,11,11,20,20,12, 12,11,00,35,20,20,00,00,04,04,04,04,04,04,04,04, 20,04,04,04,04,04,00,20,60,00,10,10,20,11,20,20},
{20,20,00,00,00,30,30,30,20,00,00,00,00,20,20,00, 12,00,11,00,20,20,20,00,04,04,04,04,04,04,04,04, 21,04,04,04,04,64,20,21,20,20,13,13,20,11,21,20},
{20,21,00,00,00,20,20,20,20,20,13,13,20,20,20,20, 12,12,11,00,21,20,20,00,00,04,04,04,04,04,04,04, 04,04,04,04,04,00,21,00,20,21,00,00,20,62,11,20},
{20,33,13,20,20,20,20,20,20,20,00,11,21,20,20,20, 20,12,11,00,00,21,20,20,00,00,04,04,04,04,04,04, 04,04,04,04,00,00,00,00,21,00,00,20,20,35,11,20},
{20,33,00,30,30,20,20,20,20,20,00,00,11,21,20,20, 20,12,00,11,00,33,20,21,20,00,00,00,04,04,04,04, 04,04,04,04,04,20,00,00,00,00,20,20,33,11,00,20},
{20,33,00,31,30,20,20,20,20,20,20,00,11,33,21,20, 20,00,12,20,11,00,20,13,20,20,64,00,04,04,04,04, 04,04,04,04,20,20,20,20,34,20,20,20,20,13,20,20},
{20,20,00,00,20,20,20,20,20,20,20,20,11,00,33,20, 20,20,12,20,11,00,21,00,21,21,04,04,04,04,04,04, 20,04,00,20,21,21,21,21,20,20,20,20,21,11,33,20},
{20,20,20,00,21,21,20,20,20,20,20,20,00,11,38,20, 20,21,12,20,11,20,30,00,00,00,61,04,04,04,04,04, 21,04,00,14,00,00,00,33,20,20,20,21,00,11,20,20},
{20,20,21,00,00,00,21,20,20,20,20,20,00,11,00,21, 20,12,12,21,11,21,30,31,00,35,20,04,04,04,04,00, 04,04,20,20,20,00,00,00,14,14,14,00,10,20,20,20},
{20,20,33,00,00,00,00,21,21,21,20,20,20,00,11,33, 21,20,12,12,11,00,20,20,13,20,20,04,04,04,04,00, 20,20,20,21,20,00,30,00,20,20,21,11,00,20,21,20},
                                                                                                 
{20,20,20,00,00,20,00,00,00,32,20,20,20,20,11,00, 33,20,12,12,00,11,21,20,13,20,20,20,04,04,04,04, 21,21,21,00,14,00,30,00,20,20,60,11,00,14,00,21},
{20,20,20,20,00,21,20,00,35,00,21,20,20,20,20,11, 00,21,12,12,20,11,33,21,00,21,20,20,20,04,04,04, 00,00,00,00,20,00,00,10,14,14,10,20,20,21,00,12},
{20,20,20,20,00,00,21,00,00,00,00,20,20,20,21,35, 11,33,20,12,21,00,11,00,00,33,20,20,21,04,04,04, 04,00,20,00,21,35,90,20,20,20,20,20,21,00,00,12},
{20,20,20,20,20,31,31,00,00,31,31,21,20,20,00,00, 11,00,14,12,12,00,11,00,33,20,20,20,04,04,04,64, 00,00,20,20,33,00,20,20,20,21,21,21,00,00,12,12},
{20,20,20,20,20,31,31,00,00,31,31,33,20,20,20,33, 11,20,20,20,12,20,35,11,00,00,20,20,04,04,04,20, 30,20,20,20,20,20,20,20,21,00,00,00,00,12,12,12},
{20,20,20,20,20,20,00,00,00,00,00,00,21,20,20,20, 11,21,20,20,12,21,00,20,13,20,21,20,04,04,04,21, 20,20,20,20,21,20,20,21,00,00,00,34,12,12,12,20},
{20,20,20,21,20,91,31,31,00,00,31,31,33,20,21,21, 00,11,21,20,12,12,12,21,11,21,33,20,20,04,04,04, 21,20,20,21,34,21,20,00,00,00,12,12,12,00,00,20},
{20,20,21,00,20,20,31,31,00,61,31,31,33,21,12,60, 00,11,33,20,12,00,12,12,00,11,00,20,20,04,04,04, 04,20,20,64,00,00,21,00,00,12,12,12,00,00,00,20},
{20,20,00,00,21,21,20,00,00,00,00,20,12,12,91,20, 00,11,00,20,12,12,00,12,12,00,11,21,20,04,04,04, 04,20,20,20,00,00,00,00,12,12,00,00,00,20,13,20},
{20,20,00,00,00,33,21,00,00,00,20,21,12,00,64,20, 20,11,00,21,20,12,12,00,12,12,11,33,21,20,04,04, 04,21,20,21,00,00,00,12,12,00,00,20,20,21,00,20},
{20,20,13,20,00,00,00,00,35,20,21,12,20,00,00,21, 20,00,11,00,21,20,20,12,00,12,00,11,32,21,04,04, 04,04,21,00,00,00,12,12,34,00,20,21,21,00,00,20},
{20,21,00,20,20,20,20,20,20,20,12,12,21,34,00,00, 20,20,00,11,33,21,21,12,12,12,12,11,31,31,04,04, 04,04,00,00,12,12,12,00,00,00,20,00,64,00,00,20},
{20,00,00,21,20,21,21,21,21,12,12,20,00,00,00,00, 14,20,20,00,11,00,00,20,20,12,12,11,20,00,21,04, 04,04,00,12,12,00,00,00,00,20,20,20,00,00,00,20},
{20,00,00,00,21,33,00,33,00,33,12,20,20,00,00,00, 20,20,21,00,00,11,00,21,20,12,12,11,21,04,04,04, 04,00,00,12,00,00,20,64,20,20,20,20,20,20,13,20},
{20,20,00,00,00,00,00,00,00,00,12,21,21,20,00,00, 21,20,33,00,20,61,11,33,20,00,12,11,04,04,04,20, 04,04,00,12,00,20,21,20,21,20,20,20,21,21,00,20},
{20,20,20,20,32,32,32,20,20,00,00,34,00,20,00,00, 00,20,20,20,20,35,00,11,20,20,12,61,11,04,04,21, 04,04,00,00,00,21,00,14,00,21,21,21,00,00,00,20},

{20,20,20,20,20,20,20,21,21,20,00,00,00,20,20,00, 00,21,20,20,20,00,00,11,20,20,12,20,11,00,04,04, 04,04,04,34,33,00,00,20,00,00,00,00,00,20,20,20},
{20,20,20,21,21,21,21,33,33,21,21,00,00,20,20,00, 00,64,20,20,20,20,13,13,20,20,12,21,11,00,00,04, 04,20,04,00,00,00,91,20,00,00,00,20,20,21,20,20},
{20,20,20,60,00,00,00,20,00,00,35,00,20,21,20,20, 00,00,14,14,20,20,00,11,21,20,12,12,20,11,04,04, 04,21,04,21,20,20,20,20,20,00,00,21,21,00,21,20},
{20,20,20,00,00,00,00,21,00,00,00,00,21,33,20,20, 00,34,20,20,20,20,20,11,00,20,12,12,21,11,00,30, 04,04,04,36,21,20,21,20,20,00,00,00,00,00,00,20},
{20,20,21,00,00,00,00,00,00,00,00,00,00,00,20,20, 00,00,21,20,20,20,20,00,11,21,20,12,00,00,10,10, 10,24,24,24,10,14,00,20,20,00,00,64,00,00,20,20},
{20,20,33,00,00,00,00,00,00,00,00,00,00,00,20,20, 00,00,00,20,20,20,20,20,11,00,20,12,12,20,00,00, 04,04,04,04,00,20,13,20,20,20,34,00,34,20,20,20},
{20,20,20,00,00,00,20,00,00,00,00,00,00,00,21,20, 20,00,00,20,20,20,20,33,11,00,14,00,12,21,00,00, 34,04,04,04,35,21,11,21,20,20,20,20,20,20,20,20},
{20,20,20,20,00,62,14,00,00,00,00,00,00,00,33,20, 20,13,13,20,20,20,20,33,00,11,21,00,12,00,35,00, 00,04,04,04,20,33,11,00,20,20,20,21,21,20,20,20},
{20,20,20,21,00,00,14,00,00,00,00,00,00,00,90,20, 21,00,00,20,20,20,20,20,20,11,33,20,20,12,20,00, 00,04,04,04,21,61,11,00,20,20,21,91,91,21,20,20},
{20,20,21,00,00,00,14,00,38,30,30,31,00,00,35,20, 00,00,20,20,20,20,20,20,21,11,00,21,21,12,21,31, 04,04,04,04,04,04,11,20,20,20,00,00,00,00,21,20},
{20,21,00,00,00,00,20,00,00,30,30,00,00,60,00,20, 00,00,20,20,21,21,21,21,00,11,00,00,12,12,00,00, 20,04,04,04,04,00,11,21,20,20,00,00,00,00,33,20},
{20,00,35,00,00,00,21,33,00,00,00,00,00,00,20,20, 34,00,20,21,30,32,32,31,31,11,20,20,12,12,00,20, 20,20,04,04,04,04,11,33,20,21,00,34,00,35,33,20},
{20,00,00,00,00,00,00,00,00,00,00,00,20,20,20,20, 20,00,20,00,31,30,31,30,00,11,21,20,00,12,00,20, 21,21,04,04,04,04,11,61,20,33,00,65,00,00,20,20},
{20,20,00,00,00,00,00,00,00,00,33,20,20,20,20,20, 20,00,14,00,00,00,00,60,00,11,33,20,20,12,00,14, 00,00,04,04,04,04,00,11,21,20,00,00,00,00,20,20},
{20,20,20,33,33,11,33,20,20,20,20,20,20,20,20,20, 20,20,20,20,20,00,35,00,00,11,11,33,20,12,12,21, 00,00,04,04,04,04,20,11,33,21,20,00,11,00,20,20},
{20,20,20,20,11,00,20,20,20,20,20,20,20,20,20,20, 20,20,20,20,20,20,20,20,00,11,31,66,33,20,12,12, 00,00,04,04,04,04,21,00,11,00,21,20,13,20,20,20},

{20,20,20,20,13,20,20,21,21,21,20,20,20,20,20,20, 20,20,20,20,20,20,20,20,20,00,11,00,21,20,20,12, 12,00,00,64,04,04,04,04,00,11,33,21,29,21,20,20},
{20,20,20,21,29,20,20,30,30,30,21,21,20,20,20,20, 20,20,20,20,20,20,20,20,20,20,11,00,31,21,20,20, 12,12,00,00,20,04,04,04,04,00,10,10,10,00,33,20},
{20,20,21,33,11,21,21,30,31,30,00,33,21,21,21,21, 21,21,20,20,20,20,20,20,20,21,11,00,31,33,21,21, 20,20,12,12,21,20,04,04,04,04,00,11,00,00,21,20},
{20,20,32,31,00,00,61,31,31,32,00,00,00,31,31,00, 35,00,21,20,20,21,20,21,21,33,11,00,00,00,35,00, 21,21,00,12,00,21,20,04,04,04,33,11,00,00,00,20},
{20,20,32,31,00,00,00,00,38,00,00,00,10,10,10,10, 10,10,00,21,21,00,21,32,00,10,00,38,00,00,00,00, 00,00,00,12,00,35,21,04,04,04,04,11,00,00,00,20},
{20,20,00,00,00,00,00,35,00,00,00,10,00,00,00,00, 00,00,10,10,10,10,10,10,10,00,00,00,00,00,00,00, 00,00,12,12,00,00,00,04,04,04,04,11,00,31,20,20},
{20,20,20,20,00,00,10,10,10,10,10,00,00,00,00,00, 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00, 00,00,00,24,00,00,04,04,04,04,66,00,11,20,20,20},
{20,20,20,21,00,10,33,00,00,00,31,00,33,00,00,00, 33,00,00,00,00,33,00,00,00,00,33,00,00,00,00,33, 00,00,34,12,00,00,00,04,04,04,00,00,11,21,21,20},
{20,20,20,00,10,00,00,00,00,31,00,20,20,00,38,00, 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00, 00,00,00,12,12,00,04,04,04,33,33,00,11,00,60,20},
{20,21,21,00,11,01,01,73,74,75,10,21,20,20,13,20, 20,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00, 00,00,00,12,12,04,04,04,04,04,00,66,11,35,00,20},
{21,33,33,30,10,10,10,10,10,10,10,33,20,21,00,21, 21,20,00,00,00,00,00,00,00,00,00,00,00,00,00,00, 00,00,12,12,04,04,20,04,04,04,00,20,00,00,20,20},
{37,10,00,10,10,10,73,74,75,10,10,33,20,00,00,60, 33,21,20,00,00,00,00,00,00,31,31,31,31,30,30,30, 00,35,12,12,04,04,21,04,04,04,00,21,00,00,20,20},
{20,33,33,11,10,10,60,10,10,10,10,33,20,20,00,00, 00,31,21,20,00,35,60,00,20,31,30,32,31,30,30,30, 00,00,12,00,00,04,04,04,04,04,04,00,00,00,21,20},
{20,20,38,11,35,34,20,20,20,30,32,20,20,20,20,20, 31,31,30,20,20,00,00,20,20,20,20,20,20,20,00,00, 00,12,20,00,00,00,04,20,04,04,04,04,00,00,33,20},
{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20, 20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,00, 12,12,20,20,20,20,00,21,04,04,04,04,00,60,20,20},
{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20, 20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20, 12,12,20,20,20,20,20,04,04,20,04,04,20,39,20,20},

            };
        }
      }
  }
                                                   