   M S B A M L   `   `   `  ����   LFSR    LFSR.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     �Name$  MainWindow1�� � Title$
 LFSR��$	��500��$	��350�� � 
ResizeMode$ NoResize=�.��� .+��� $
��Left=�$	��Top=�$	��460��$	��330������ .@��� $��LFSR Generator��.��� $	��437��$	��Top=�����  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ 	#FF0F0763�� � Offset$ 0Q�� $ 	#FF311CFD��$ 1Q�.+��� -    ��Name$ IMG_generator��$
��Left=�$��30��$��	270,5,0,0q�$	��Top=�$��40��$��help-icon.png���� -    ��Name$ LBL_select_option��$��Select option��$
��Left=�$��	10,10,0,0q�$	��Top=�$��30��$	��200��
��������� -   	 ��Name$	 
CB_options��$
��Left=�$��34��$��	10,35,0,0q�$	��Top=�$	��300��
 � ToolTip$(
 "Select options from the list below���� -   $ 
LBL_amount��$��Amount��$
��Left=�$��	10,69,0,0q�$	��Top=�$	��200��$��30��
��������� -    ��Name$ 	TB_amount��$��	10,99,0,0q� �� TextWrapping$
 Wrap=�$��10000��$	��Top=�$	��300��$��36��$
 Enter number of repeats��$
��Left=��� -   $ 	LBL_input��$��Input��$
��Left=�$��
10,140,0,0q�$	��Top=�$��30��$	��200��
��������� -   $ TB_input��$
��Left=�$��
10,170,0,0q�$
 Wrap=�$	��Top=�$��84��$	��300��$
 Enter your registry data���� -	   $ 
LBL_output��$��Output��$
��Left=�$��
10,259,0,0q�$	��Top=�$��30��$	��200��
��������� -
   $ TB_generate_output��$
��Left=�$��
10,294,0,0q�$
 Wrap=�$	��Top=�$��84��$	��300�� �� 
IsReadOnly . $
 It's your result���� -    ��Name$ BT_generate��$��Generate��$
��Left=�$��
10,383,0,0q�$	��300��{�.  $"
 Click after selecting option��$��40��$	��Top=����� $ 0.5,1&�$ 0.5,0&� �� Opacity$	 0.8Q�.��� $ 	#FF363636��$ 0Q�� $ 	#FF363636��$ 1Q�� $ White��$	 0.5Q��� $��
XOR Cipher��.��� $	��500��$	��Top=����� $ 0.5,1&�$ 0.5,0&�.��� $ 	#FF0F0763��$ 0Q�� $ 	#FF311CFD��$ 1Q�.+��� -   $ 
IMG_cipher��$
��Left=�$��30��$��
270,10,0,0q�$	��Top=�$��40��$��help-icon.png�� J� RenderTransformOrigin$ 7.258,0.511&��� -   $ LBL_select_file��$��Select file��$
��Left=�$��	10,10,0,0q�$	��Top=�$��30��$	��200��
��������� -   $ TB_select_file��$
��Left=�$��	10,45,0,0q�$
 Wrap=�$	��Top=�$��30��$	��200��$
 Select file�� . �� -   $ BT_select_file��$��Select file��$
��Left=�$��30��$��
216,45,0,0q�$	��Top=�$��94��$ 0.505,1.333&����� $ 0.5,1&�$ 0.5,0&�$	 0.8Q�.��� $ 	#FF363636��$ 0Q�� $ 	#FF363636��$ 1Q�� $ White��$	 0.5Q�	� -    	�Name$ 
RB_encrypt��$��Encrypt��$
��Left=�$��	10,80,0,0q�$	��Top=�$��30��$	��100��
�������	� -   $ 
RB_decrypt��$��Decrypt��$
��Left=�$��
167,80,0,0q�$	��Top=�$��30��$	��100��
��������� -   $ LBL_key��$	��Key��$
��Left=�$��	10,92,0,0q�$	��Top=�$��30��$	��200��
��������� -   $ TB_key��$
��Left=�$��
10,127,0,0q�$
 Wrap=�$	��Top=�$�� ��$%
 Result of encryption/decryption��$��30��$	��200�� .  �� -   $ BT_generate_random��$��Generate key��$
��Left=�$��30��$��216,127,0,0q�$	��Top=�$��94��$ 0.505,1.333&����� $ 0.5,1&�$ 0.5,0&�$	 0.8Q�.��� $ 	#FF363636��$ 0Q�� $ 	#FF363636��$ 1Q�� $ White��$	 0.5Q��� -   $ LBL_output_encryptor��$��Output��$
��Left=�$��
10,172,0,0q�$	��Top=�$��30��$	��200��
��������� -   $ TB_encrypt_output��$
��Left=�$��
10,207,0,0q�$
 Wrap=�$	��Top=�$�� ��$%
 Result of encryption/decryption��$	��100��$	��300�� .  �� -   $ 	BT_cipher��$��Start��$
��Left=�$��
10,312,0,0q�$	��300��{�.  $*
 $Click to start encryption/decryption��$��40��$	��Top=����� $ 0.5,1&�$ 0.5,0&�$	 0.8Q�.��� $ 	#FF363636��$ 0Q�� $ 	#FF363636��$ 1Q�� $ White��$	 0.5Q��� $��Tests��.��� $	��500��$	��Top=����� $ 0.5,1&�$ 0.5,0&�.��� $ 	#FF0F0763��$ 0Q�� $ 	#FF311CFD��$ 1Q�.+��� -   $ LBL_select_test��$��Select test��$
��Left=�$��	10,10,0,0q�$	��Top=�$��30��$	��200��
��������� -   $ IMG_test��$
��Left=�$��30��$��
270,10,0,0q�$	��Top=�$��40��$��help-icon.png��$ 7.258,0.511&�	� -   $ RB_individual_bits��$��Individual bits��$
��Left=�$��
179,45,0,0q�$	��Top=�$��30��$	��100��
�������	� -   $ RB_pair_bits��$��	Pair bits��$
��Left=�$��
179,75,0,0q�$	��Top=�$��30��$	��100��
�������	� -   $ RB_poker��$��Poker��$
��Left=�$��	10,75,0,0q�$	��Top=�$��30��$	��100��
�������	� -   $ RB_long_series��$��Long series��$
��Left=�$��	10,45,0,0q�$	��Top=�$��30��$	��100��
������� d� 	IsChecked$
 TrueY��� -   $ LBL_key_test��$��Key (30,6,4,1,0)��$
��Left=�$��	10,92,0,0q�$	��Top=�$��30��$	��200��
��������� -   $ TB_key_test��$
��Left=�$��
10,127,0,0q�$
 Wrap=�$	��Top=�$�� ��$	
 Key��$	��100��$	��300�� .  �� -    $ LBL_test_output��$��Output��$
��Left=�$��
10,231,0,0q�$	��Top=�$��30��$	��200��
��������� -!   $ TB_test_output��$
��Left=�$��
10,266,0,0q�$
 Wrap=�$	��Top=�$�� ��$
 Result of testing��$��40��$	��300�� .  �� -"   $ BT_test��$
��Test��$
��Left=�$��
10,312,0,0q�$	��300��$
 Click to start testingn��$��40��$	��Top=����� $ 0.5,1&�$ 0.5,0&�$	 0.8Q�.��� $ 	#FF363636��$ 0Q�� $ 	#FF363636��$ 1Q�� $ White��$	 0.5Q�