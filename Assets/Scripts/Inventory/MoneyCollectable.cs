using UnityEngine;

[RequireComponent(typeof(Collectable))]
[RequireComponent(typeof(Destroyer))]
public class MoneyCollectable : MonoBehaviour
{
    [SerializeField][Range(1, 100)] int moneyValue = 1;
    [SerializeField][Range(1,100)] float speed = 10;

    Collectable Collectable => GetComponent<Collectable>();
    Destroyer Destroyer => GetComponent<Destroyer>();

    bool movingToCollector = false;
    Inventory inventory;

    private void Start()
    {
        Collectable.OnCollectCallback += Collect;
    }

    void Collect(Collector collector)
    {
        if (movingToCollector) return;

        inventory = collector.GetComponent<Inventory>();
        if (inventory != null)
        {
            movingToCollector = true;
        }
    }

    private void FixedUpdate()
    {
        if (movingToCollector)
        {
            transform.position = Vector3.MoveTowards(transform.position, inventory.transform.position, speed * Time.fixedDeltaTime);
            if ((transform.position - inventory.transform.position).sqrMagnitude < 0.25f)
            {
                inventory.ChangeMoney(moneyValue);
                Destroyer.Destroy();
            }
        }
    }


}
