# Magic Wheel

Demo video: <https://youtu.be/tBRXrxkypMo>

## Packet Structures

### Server Packets

With `register_status` is [ 0x01 (successful) | 0x02 (invalid) | 0x03 (already exists) ]

With `score_board` = <player_count><player1_name_length><player1_name><player1_score>...<playerN_name_length><playerN_name><playerN_score>

With `next_player_turn` = <next_player_name_len><next_player_name>

With `result_keyword` = <keyword_length><result_keyword>

- RegisterResp: <0x01><register_status>
- NewPlayerInform: <0x02><player_name_length><player_name>
- StartGame: <0x03><turn_number><answer_length><description_length><description><next_player_turn>
- PlayerTurn: <0x04><turn_number><result_keyword><next_player_turn>
- CorrectChar: <0x05><turn_number><result_keyword><score_board><next_player_turn>
- EndGame: <0x06><keyword_length><result_keyword><score_board>

### Client Packets

- Register: <0x01><name_length><name>
- Answer: <0x02><character><keyword_length><keyword>
