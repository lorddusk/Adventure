using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;

namespace Adventure
{
    public partial class Adventure : Form
    {
        private Player _player;
        private Monster _currentMonster;

        public Adventure()
        {
            InitializeComponent();
            _player = new Player(10, 10, 20, 0, 1);
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            _player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));

            lblHitPoints.Text = _player.CurrHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.XpPoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
        }

        private void MoveTo(Location newLocation)
        {
            if (!_player.HasRequiredItemToEnterThisLocation(newLocation))
            {
                rtbMessages.Text += "You must have a " + newLocation.ItemRequiredtoEnter.Name +
                                    " to enter this location." + Environment.NewLine;
                return;
            }

            _player.CurrentLocation = newLocation;

            btnNorth.Visible = (newLocation.LocationToNorth != null);
            btnEast.Visible = (newLocation.LocationToEast != null);
            btnSouth.Visible = (newLocation.LocationToSouth != null);
            btnWest.Visible = (newLocation.LocationToWest != null);

            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text += newLocation.Description + Environment.NewLine;

            _player.CurrHitPoints = _player.MaxHitPoints;

            lblHitPoints.Text = _player.CurrHitPoints.ToString();

            if (newLocation.QuestAvailableHere != null)
            {
                bool playerAlreadyHasQuest = _player.HasThisQuest(newLocation.QuestAvailableHere);
                bool playerAlreadyHasCompletedQuest = _player.CompletedThisQuest(newLocation.QuestAvailableHere);

                if (playerAlreadyHasQuest)
                {
                    if (!playerAlreadyHasCompletedQuest)
                    {
                        bool playerHasAllItemsToCompleteQuest =
                            _player.HasAllQuestCompletionItems(newLocation.QuestAvailableHere);

                        if (playerHasAllItemsToCompleteQuest)
                        {
                            rtbMessages.Text += Environment.NewLine;
                            rtbMessages.Text += "You complete the " + newLocation.QuestAvailableHere.Name +
                                                " quest." + Environment.NewLine;

                            _player.RemoveQuestCompletionItems(newLocation.QuestAvailableHere);

                            rtbMessages.Text += "You receive: " + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardXp.ToString() +
                                                " experience points" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardGold.ToString() + " gold" +
                                                Environment.NewLine;
                            rtbMessages.Text +=
                                newLocation.QuestAvailableHere.RewardItem.Name + Environment.NewLine;
                            rtbMessages.Text += Environment.NewLine;

                            _player.XpPoints += newLocation.QuestAvailableHere.RewardXp;
                            _player.Gold += newLocation.QuestAvailableHere.RewardGold;

                            _player.AddItemToInventory(newLocation.QuestAvailableHere.RewardItem);

                            _player.MarkQuestCompleted(newLocation.QuestAvailableHere);
                        }
                    }
                }
                else
                {
                    rtbMessages.Text += "You receive the " + newLocation.QuestAvailableHere.Name + " quest." +
                                        Environment.NewLine;
                    rtbMessages.Text += newLocation.QuestAvailableHere.Description + Environment.NewLine;
                    rtbMessages.Text += "To complete it, return with: " + Environment.NewLine;
                    foreach (QuestCompletionItem questCompletionItem in newLocation.QuestAvailableHere
                        .QuestCompletionItems)
                    {
                        if (questCompletionItem.Amount == 1)
                            rtbMessages.Text += questCompletionItem.Amount.ToString() + " " +
                                                questCompletionItem.Details.Name + Environment.NewLine;
                        else
                            rtbMessages.Text += questCompletionItem.Amount.ToString() + " " +
                                                questCompletionItem.Details.NamePlural + Environment.NewLine;
                    }

                    rtbMessages.Text += Environment.NewLine;

                    _player.Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
                }
            }

            if (newLocation.MonsterLivingHere != null)
            {
                rtbMessages.Text += "You see a " + newLocation.MonsterLivingHere.Name + Environment.NewLine;

                Monster monster = World.MonsterByID(newLocation.MonsterLivingHere.ID);
                _currentMonster = new Monster(monster.MaxDamage, monster.RewardXp, monster.ID, monster.Name,
                    monster.RewardGold, monster.CurrHitPoints, monster.MaxHitPoints);

                foreach (LootItem lootItem in monster.LootTable)
                {
                    _currentMonster.LootTable.Add(lootItem);
                }

                cbWeapons.Visible = true;
                cbPotions.Visible = true;
                btnUseWeapon.Visible = true;
                btnUsePotion.Visible = true;
            }
            else
            {
                _currentMonster = null;

                cbWeapons.Visible = false;
                cbPotions.Visible = false;
                btnUseWeapon.Visible = false;
                btnUsePotion.Visible = false;
            }

            UpdateInventoryListInUI();

            UpdateQuestListInUI();

            UpdateWeaponListInUI();

            UpdatePotionListInUI();
        }

        private void UpdateInventoryListInUI()
        {
            dgvInventory.RowHeadersVisible = false;
            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Name";
            dgvInventory.Columns[0].Width = 197;
            dgvInventory.Columns[1].Name = "Quantity";
            dgvInventory.Rows.Clear();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Amount > 0)
                    dgvInventory.Rows.Add(new[]
                    {
                        inventoryItem.Details.Name, inventoryItem.Amount.ToString()
                    });
            }
        }

        private void UpdateQuestListInUI()
        {
            dgvQuests.RowHeadersVisible = false;
            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Name";
            dgvQuests.Columns[0].Width = 197;
            dgvQuests.Columns[1].Name = "Done?";
            dgvQuests.Rows.Clear();

            foreach (PlayerQuest playerQuest in _player.Quests)
            {
                dgvQuests.Rows.Add(new[]
                {
                    playerQuest.Details.Name, playerQuest.isCompleted.ToString()
                });
            }
        }

        private void UpdateWeaponListInUI()
        {
            List<Weapon> weapons = new List<Weapon>();
            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Details is Weapon)
                    if (inventoryItem.Amount > 0)
                        weapons.Add((Weapon) inventoryItem.Details);
            }

            if (weapons.Count == 0)
            {
                cbWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cbWeapons.DataSource = weapons;
                cbWeapons.DisplayMember = "Name";
                cbWeapons.ValueMember = "ID";
                cbWeapons.SelectedIndex = 0;
            }
        }

        private void UpdatePotionListInUI()
        {
            List<HealingPotion> healingPotions = new List<HealingPotion>();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Details is HealingPotion)
                    if (inventoryItem.Amount > 0)
                        healingPotions.Add((HealingPotion) inventoryItem.Details);
            }

            if (healingPotions.Count == 0)
            {
                cbPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                cbPotions.DataSource = healingPotions;
                cbPotions.DisplayMember = "Name";
                cbPotions.ValueMember = "ID";
                cbPotions.SelectedIndex = 0;
            }
        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            Weapon currentWeapon = (Weapon) cbWeapons.SelectedItem;

            int damageToMonster = RandomGenerator.NumberBetween(currentWeapon.MinDamage, currentWeapon.MaxDamage);

            _currentMonster.CurrHitPoints -= damageToMonster;

            rtbMessages.Text += "You hit the " + _currentMonster.Name + " for " + damageToMonster.ToString() +
                                " points." + Environment.NewLine;

            if (_currentMonster.CurrHitPoints <= 0)
            {
                rtbMessages.Text += Environment.NewLine;
                rtbMessages.Text += "You defeated the " + _currentMonster.Name + Environment.NewLine;

                _player.XpPoints += _currentMonster.RewardXp;
                rtbMessages.Text += "You receive " + _currentMonster.RewardXp.ToString() + " experience points" +
                                    Environment.NewLine;

                _player.Gold += _currentMonster.RewardGold;
                rtbMessages.Text += "You receive " + _currentMonster.RewardGold.ToString() + " gold" +
                                    Environment.NewLine;

                List<InventoryItem> lootedItems = new List<InventoryItem>();

                foreach (LootItem lootItem in _currentMonster.LootTable)
                {
                    if (RandomGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage)
                        lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                }

                if (lootedItems.Count == 0)
                {
                    foreach (LootItem lootItem in _currentMonster.LootTable)
                    {
                        if (lootItem.isDefaultItem)
                            lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                    }
                }

                foreach (InventoryItem inventoryItem in lootedItems)
                {
                    _player.AddItemToInventory(inventoryItem.Details);

                    if (inventoryItem.Amount == 1)
                        rtbMessages.Text += "You loot " + inventoryItem.Amount.ToString() + " " +
                                            inventoryItem.Details.Name +
                                            Environment.NewLine;
                    else
                        rtbMessages.Text += "You loot " + inventoryItem.Amount.ToString() + " " +
                                            inventoryItem.Details.NamePlural + Environment.NewLine;
                }

                lblHitPoints.Text = _player.CurrHitPoints.ToString();
                lblGold.Text = _player.Gold.ToString();
                lblExperience.Text = _player.XpPoints.ToString();
                lblLevel.Text = _player.Level.ToString();

                UpdateInventoryListInUI();
                UpdateWeaponListInUI();
                UpdatePotionListInUI();

                rtbMessages.Text += Environment.NewLine;

                MoveTo(_player.CurrentLocation);
            }
            else
            {
                int damageToPlayer = RandomGenerator.NumberBetween(0, _currentMonster.MaxDamage);

                rtbMessages.Text += "The " + _currentMonster.Name + " did " + damageToPlayer.ToString() +
                                    " points of damage." + Environment.NewLine;

                _player.CurrHitPoints -= damageToPlayer;

                lblHitPoints.Text = _player.CurrHitPoints.ToString();

                if (_player.CurrHitPoints <= 0)
                {
                    rtbMessages.Text += "The " + _currentMonster.Name + " killed you." + Environment.NewLine;
                    MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
                }
            }
        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            HealingPotion potion = (HealingPotion) cbPotions.SelectedItem;

            _player.CurrHitPoints = (_player.CurrHitPoints + potion.AmountToHeal);

            if (_player.CurrHitPoints > _player.MaxHitPoints)
                _player.CurrHitPoints = _player.MaxHitPoints;

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Details.ID == potion.ID)
                {
                    inventoryItem.Amount--;
                    break;
                }
            }

            rtbMessages.Text += "You drink a " + potion.Name + Environment.NewLine;

            int damageToPlayer = RandomGenerator.NumberBetween(0, _currentMonster.MaxDamage);

            rtbMessages.Text += "The " + _currentMonster.Name + " did " + damageToPlayer.ToString() +
                                " points of damage." + Environment.NewLine;

            _player.CurrHitPoints -= damageToPlayer;

            lblHitPoints.Text = _player.CurrHitPoints.ToString();

            if (_player.CurrHitPoints <= 0)
            {
                rtbMessages.Text += "The " + _currentMonster.Name + " killed you." + Environment.NewLine;
                MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            }

            lblHitPoints.Text = _player.CurrHitPoints.ToString();

            UpdateInventoryListInUI();
            UpdatePotionListInUI();
        }
    }
}