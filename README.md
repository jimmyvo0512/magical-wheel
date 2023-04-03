# Magic Wheel

## Packet Structures

### Server Packets

With `score_board` = <player_count><player1_name_length><player1_name><player1_score>...<playerN_name_length><playerN_name><playerN_score>

- RegisterResp: <0x01><status>
- NewPlayerInform: <0x02><player_name_length><player_name>
- StartGame: <0x03><answer_length><description_length><description><player_name_length><player_name>
- PlayerTurn: <0x04><player_name_length><player_name>
- CorrectChar: <0x05><char><score_board><next_player_name_length><next_player_name>
- EndGame: <0x06><keyword_length><result_keyword><score_board>

### Client Packets

- Register: <0x01><name_length><name>
- Answer: <0x02><character><keyword_length><keyword>
