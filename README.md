# 🎰 Roulette Game - README

https://github.com/user-attachments/assets/2298d0bc-191a-4e4f-b096-eee2cafa6e94

## 🎮 Controls and Gameplay Instructions

To play the game:

1. **Chip Selection**: Select one of the different chip values on the screen.
2. **Bet Placement**: Place your chosen chip by clicking on the desired bet fields on the table. You can place more than one chip on the same field.
3. **Start Spin**:
   - You can start the game by simply pressing the “Spin” button.
   - Or, you can start the game by entering a number you want to be displayed before pressing the “Spin” button (in this case the game will return to the target number).
4. **Winner Calculation**: When the ball lands on a number, winning bets are automatically calculated and winnings are added to the player's balance.

## 🧩 System Features

- **Data Saving**: Bets placed and the total balance are retained even if the game is closed.
- Multiple Bet Support**: Chips can be placed in different areas at the same time.
- **Advanced Sound System**: Spin sound decreases over time and adds dynamic atmosphere to the game.
- **Targeted Spin**: Special scenarios can be tried with a spin system focused on a specific number.

## ⚙️ Future Improvements:

1.**Automated Stack Management**: A smart system will be developed to automatically optimize the stacks (tokens) in the betting area.

-How will  work?

Automatic Consolidation: 10 chip of the same value will be automatically converted to the next higher value coin
Example Scenario:

10x1 chips → 1x10 chip
10x5 chips → 1x50 chip

Smart Optimization: The system will automatically select the most efficient combination of coins

🎯 Benefits

Cleaner and more organized betting area
Reduced visual clutter
Better experience for players
Professional casino atmosphere

Translated with DeepL.com (free version)

2. **📊 Roulette Wheel Statistics Display** :Statistics will be displayed with an interactive roulette wheel design instead of the current bar charts.

## OOP Principles:

**1. Encapsulation**:

   - Private fields with controlled access across all components
   - Unity SerializeField integration maintaining data protection
   - Thread-safe operations with proper synchronization
   - Constructor-based dependency injection for secure initialization

**2. Abstraction**:

   - Multiple interface layers (IObjectPool<T>, IInteractable, ISaveService<T>)
   - Abstract base classes providing extensible templates
   - Generic type systems with appropriate constraints
   - Complex operations simplified through clean API surfaces

**3. Inheritance**:

   - Hierarchical class structures with virtual method overriding
   - MonoBehaviour integration for Unity-specific functionality
   - Template method patterns in base classes
   - Specialized implementations extending core functionality

**4. Polymorphism**:

   - Interface-driven design enabling interchangeable implementations
   - Generic programming supporting multiple data types
   - Runtime type identification and casting
   - Uniform method signatures handling diverse object behaviors

**5. SOLID Principles**:

   - Single Responsibility: Each class manages one specific concern
   - Open/Closed: System extensible through new implementations without modification
   - Liskov Substitution: Derived classes fully compatible with base abstractions
   - Interface Segregation: Focused, minimal interface contracts
   - Dependency Inversion: High-level modules depend on abstractions, not concretions

**6. Design Patterns**:

   - Object Pooling pattern for memory optimization
   - Template Method pattern in base classes
   - Strategy pattern through interface implementations
