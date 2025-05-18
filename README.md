![Hexen Thumbnail](https://github.com/user-attachments/assets/edc42f44-b2f7-4eca-996c-fc6cc579bb5e)

This project helped me understand clean code and scalable game architecture. The game is small hexagon board game. 
The gameplay consists of dragging cards onto enemy pieces on the board. Each card has a different moveset that has its own behaviour. 
The movesets are: 

1. Ring
2. Asteroid
3. Slash
4. Teleport
5. Pushback
6. Rain
7. Blitz

The game ends when there are no enemy pieces left on the board. 

![Gameloop Gif](https://github.com/user-attachments/assets/3d7767be-2a33-4249-a811-84708a359bb2)
[UPDATED PROJECT FOOTAGE]

Originally the Hexen board game was a project that I developed during my studies in Independent Game Production.
I learned how to use events (C# + Unity) which was a real eye opener for me. I learned a lot about the Model/View approach in systems design. 
This was very interesting because I learned how to decouple systems by applying the Single Responibility Principle and the Observer pattern. 
During development I made sure each class had its own distinct, independently functioning purpose and decoupled Input logic, game logic from the visual side of the game.
The project is designed to be modular and easy to extend. 

Recently I revisited the project and added a few features that I felt were missing. 

These were: 

1. Implementing undo functionality, built with the Command pattern.
2. Implementing simple game states utilizing the State pattern.
3. Player feedback enhancements with Particle effects, plug and play audio and Camera shake.

![Undo_Gif](https://github.com/user-attachments/assets/603f1b31-fbef-4ab7-b4d5-a71ce7f3aef7)
[UNDO MOVE]

If you want to try out the game yourself please do :) 

https://ricocriel.github.io/Hexen_Game/





