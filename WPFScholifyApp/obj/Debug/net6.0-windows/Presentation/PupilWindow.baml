   M S B A M L   `   `   `  ����I  EWPFScholifyApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WPFScholifyAppWPFScholifyApp  5   	        WPFScholifyApp.PupilWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:WPFScholifyApp     5   	     � Title$  Scholify��5   	   $	��450��6   $	��800��6'   
����  �.��5	      (�  (� Stretch$ Uniform=�6   .��5
   
   �  J� RenderTransformOrigin$ 
0.61,0.727&�6   $
��1841��62   .+�5      �� $h��bC:\Users\Ярослава\source\repos\WPFScholifyApp\WPFScholifyApp\Presentation\imagesavatar.jpg��6   $
��Left=�6x   $	��Top=�6�   $	��196��6�   $	��148��6�   $��
144,76,0,0q�6�   �� 5      $	��Top=�6   $
��Left=�61   $	��349��6L   $	��118��6X   $��
40,256,0,0q�6e   $ 0.732,0.588&�6y   .+�5      �� -   5         ��Name$ FirstNameTextBlock��5      $	��Top=�68   $��Center=�6P   
�������$��30(�6�   �� 5      -   5        $ LastNameTextBlock��5      $	��Top=�67   $��Center=�6O   
�������$��30(�6�   5      �� 5      $	��Top=�6   $
��Left=�61   $	��416��6L   $	��480��6X   $��
25,407,0,0q�6e   .+�5      �� -   5        $��Журнал��5      $��30(�6*   $��70��6T   
����  �
�������
����  �6�   �� 5      -   5        $��Розклад��5      $��30(�6+   $��0 20 0 0q�6V   $��70��6h   
����  �
�������
����  �6�   �� 5      -   5        $#��Дошка оголошень��5      $��30(�63   $��0 20 0 0q�6c   $��70��6u   
����  �
�������
����  �6�   �� 5      -   5        $+��%Приватна інформація��5      $��30(�67   $��0 20 0 0q�6e   $��70��6w   
����  �
�������
����  �6�   5      �� 5      -   5        $��
leftScroll��5   S   $��
486,0,10,7q�6   $ 0.358,0.498&�6/   $
��True��6e   .��5      �� -   5         ��Name$ Panel��5      $ 0.297,0.786&�6,   
�������$
��1372��6c   $
��1012��6p   5      5      �� 5      -	   5        $ Schedule��5       �� Orientation$ Vertical=�6+   $
��1023��6C   $��	486,0,0,0q�6Q   $
��1372��6d   $ 0.503,0.796&�6q   
�������$y�Hidden=�6�   .+�5      �� $��Center=�6   $��Center=�63   $��30(�6P   .��6^   Розклад6g   �� 5      $ 
Horizontal=�6   $	��900��66   .+�5      �� -
   5        $ Monday��5   !   $	��250��61   
�������$��
15,20,0,10q�6T   5      �� 5       -   5        $ Tuesday��5    !   $	��250��62   
�������$��
15,20,0,10q�6U   5!      �� 5#      -   5        $ 	Wednesday��5#   !   $	��250��65   
�������$��
15,20,0,10q�6X   5$      �� 5&      -   5        $ Thursday��5&   !   $	��250��63   
�������$��
15,20,0,10q�6V   5'      �� 5)      -   5        $ Friday��5)   !   $	��250��61   
�������$��
15,20,0,10q�6T   5*      5,      �� 5-      $��58��6   $
��1370��6)   $��Right=�66   .+�5.      �� -   5         ��Name$ previousWeek��5.      $��30(�63   $	��427��6A   $
��Left=�6M   
 � Tag$ -7��6h   .��6�   %#Попередній тиждень6�   �� 5/      -   5        $ nextWeek��5/      $��30(�6/   $	��427��6=   $'��!Наступний тиждень��6I   $��Right=�6e   $	��Top=�6�   $��	0,-45,0,0q�6�   $ 7��6�   6�   50      51      �� 53      -   5        $��
Вихід��53      $��30(�6%   $��Bottom=�6L   $
��Left=�6g   $��
141,0,0,37q�6�   
����  �
�������$	��193��6�   $��55��6�   54      55      56      