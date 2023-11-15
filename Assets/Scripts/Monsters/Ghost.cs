public class Ghost : Monster
{
    private int health;
    private int speed;

    public Monster clone()
    {
        return new Ghost();
    }
}