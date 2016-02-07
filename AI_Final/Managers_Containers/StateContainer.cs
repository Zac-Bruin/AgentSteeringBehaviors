
namespace AI_Final
{
    public class StateContainer
    {
        public Fleeing flee;
        public Hunting hunt;
        public Wandering wander;

        public StateContainer()
        {
            flee = new Fleeing();
            hunt = new Hunting();
            wander = new Wandering();
        }

    }
}
