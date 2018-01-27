package com.example.dibby.restaurantapp2;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.text.Editable;
import android.text.TextWatcher;
import android.text.method.ScrollingMovementMethod;
import android.util.Log;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.PopupMenu;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.VolleyLog;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;

import static com.example.dibby.restaurantapp2.R.id.orderNumberTextView;

public class OrderActivity extends AppCompatActivity {

    private Button addDrinkButton;
    private TextView summaryTextView;
    int tableNumber = 0;
    private EditText tableValue;
    String[][] foodItems;
    String[][] drinkItems;
    String serverURL;
    Spinner spinner;
    String tableStatus;
    String orderNumber;
    TextView orderNumberText;
    TextView getSummaryTextView;
    String[][] itemsToBeOrdered;
    int currentOrderItems = 0;
    int count = 0;
    EditText tableNumberText;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_order);

        // Load food and drink array data
        Bundle bundle = getIntent().getExtras();
        foodItems = (String[][]) bundle.getSerializable("foodItems");
        drinkItems = (String[][]) bundle.getSerializable("drinkItems");
        serverURL = bundle.getString("serverURL");
        Log.d("DEBUG: ", "food items = " + foodItems.length);
        Log.d("DEBUG: ", "drink items = " + drinkItems.length);

        summaryTextView = (TextView) findViewById(R.id.summaryTextView);
        summaryTextView.setMovementMethod(new ScrollingMovementMethod());

        orderNumberText = (TextView) findViewById(orderNumberTextView);

        itemsToBeOrdered = new String[100][6];

        tableNumberText = (EditText) findViewById(R.id.tableNumberText);
        tableNumberText.addTextChangedListener(new TextWatcher() {
            public void afterTextChanged(Editable s) {}
            public void beforeTextChanged(CharSequence s, int start,
                                          int count, int after) {}

            public void onTextChanged(CharSequence s, int start,
                                      int before, int count) {
                try {
                    tableNumber = Integer.parseInt(s.toString());
                }
                catch (NumberFormatException e){
                    tableNumber = 0;
                }
                Log.d("DEBUG: ", "Selected table is " + s.toString());
                if(isTableValid(tableNumber)){
                    summaryTextView.setText("");
                    checkIfTableIsOpen(serverURL);
                }
            }
        });

    }

    // Called when user taps Add Drink button
    public void addDrink(View view) {
        showPopupMenu(view);
    }

    // Called when user taps Add Food button
    public void addFood(View view) {
        showPopupMenu(view);
    }

    // Called whe Submit Order button is selected
    public void submitOrder(View view) {
        if(isTableValid(tableNumber) && currentOrderItems > 0){
            for(int i = 0; i < currentOrderItems; i++)
            {
                RequestQueue requestQueue = Volley.newRequestQueue(this);
                String URL = serverURL + "/ordersystem.php";
                Log.d("DEBUG: ", "Sending request to server: " + URL);

                StringRequest stringRequest = new StringRequest(Request.Method.POST, URL,
                        new Response.Listener<String>() {
                            @Override
                            public void onResponse(String response) {
                                Log.i("VOLLEY", response);
                                if(count == currentOrderItems){
                                    itemsToBeOrdered = null;
                                    itemsToBeOrdered = new String[100][6];
                                    count = 0;
                                    currentOrderItems = 0;
                                    Toast.makeText(getApplicationContext(),
                                            "Order submitted successfully.", Toast.LENGTH_SHORT).show();

                                }

                            }
                        },
                        new Response.ErrorListener() {
                            @Override
                            public void onErrorResponse(VolleyError error) {
                                Log.e("VOLLEY", error.toString());
                            }
                        }) {
                    @Override
                    protected Map<String, String> getParams() {
                        Map<String, String> params = new HashMap<String, String>();

                        params.put("menuItem", itemsToBeOrdered[count][0]);
                        params.put("userName", itemsToBeOrdered[count][1]);
                        params.put("quantity", itemsToBeOrdered[count][2]);
                        params.put("orderNumber", itemsToBeOrdered[count][3]);
                        params.put("price", itemsToBeOrdered[count][4]);
                        count++;
                        return params;
                    }
                };
                requestQueue.add(stringRequest);
            }
        }
    }


    // Called when the add food or drink buttons are pressed.
    public void showPopupMenu(View anchor) {
        Log.d("DEBUG", "showPopupMenu method entered.");
        PopupMenu menu = new PopupMenu(this, anchor);

        switch (anchor.getId()) {
            case R.id.addDrinkButton:
                for (int i = 0; i < drinkItems.length; i++) {
                    menu.getMenu().add(1, i, i, drinkItems[i][1]);
                }
                break;
            case R.id.addFoodButton:
                for (int i = 0; i < foodItems.length; i++) {
                    menu.getMenu().add(2, i, i, foodItems[i][1]);
                }
                break;
        }

        menu.setOnMenuItemClickListener(new PopupMenu.OnMenuItemClickListener() {

            @Override
            public boolean onMenuItemClick(MenuItem item) {
                //tableNumber = Integer.parseInt(spinner.getSelectedItem().toString());
                // Make sure the user is not selecting table 0
                if (tableNumber == 0  || tableNumber > 10)
                    Toast.makeText(getApplicationContext(),
                            "Invalid Table Number", Toast.LENGTH_SHORT).show();
                else {
                    Toast.makeText(getApplicationContext(),
                            item.getTitle(), Toast.LENGTH_SHORT).show();
                    if (item.getGroupId() == 1) {
                        summaryTextView.append("\n" + "Table: " + tableNumber +
                                " --> " + item.getTitle() + " - $" + drinkItems[item.getItemId()][2]);
                        // 0 menuItem, 1 username, 2 quantity, 3 orderNumber, 4 price
                        itemsToBeOrdered[currentOrderItems][0] = item.getTitle().toString();
                        itemsToBeOrdered[currentOrderItems][1] = "userName";
                        itemsToBeOrdered[currentOrderItems][2] = "1";
                        itemsToBeOrdered[currentOrderItems][3] = orderNumber;
                        itemsToBeOrdered[currentOrderItems][4] = drinkItems[item.getItemId()][2];
                        currentOrderItems++;
                    } else {
                        summaryTextView.append("\n" + "Table: " + tableNumber +
                                " --> " + item.getTitle() + " - $" + foodItems[item.getItemId()][2]);
                        // 0 menuItem, 1 username, 2 quantity, 3 orderNumber, 4 price
                        itemsToBeOrdered[currentOrderItems][0] = item.getTitle().toString();
                        itemsToBeOrdered[currentOrderItems][1] = "userName";
                        itemsToBeOrdered[currentOrderItems][2] = "1";
                        itemsToBeOrdered[currentOrderItems][3] = orderNumber;
                        itemsToBeOrdered[currentOrderItems][4] = foodItems[item.getItemId()][2];
                        currentOrderItems++;
                    }
                }
                return true;
            }
        });
        menu.show();
    }

    public void checkIfTableIsOpen(String serverURL) {
        RequestQueue queue = Volley.newRequestQueue(this);
        String url = serverURL + "/tableStatus.php";

        JsonObjectRequest req = new JsonObjectRequest(Request.Method.GET, url,
                null, new Response.Listener<JSONObject>() {

            @Override
            public void onResponse(JSONObject response) {
                Log.d("DEBUG", response.toString());
                String JSONstring;
                JSONstring = response.toString();
                getTableStatus(JSONstring);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                VolleyLog.d("DEBUG", "Error: " + error.getMessage());
                Log.e("EXCEPTPTION", "Site Info Error: " + error.getMessage());
                Toast.makeText(getApplicationContext(), "Data load failure - check URL", Toast.LENGTH_LONG).show();
            }
        });

        queue.add(req);
    }

    public void getTableStatus(String JSONstring) {
        try {
            // Create the root JSONObject from the JSON string.
            JSONObject jsonRootObject = new JSONObject(JSONstring).getJSONObject("object");

            //Get the instance of JSONArray that contains JSONObjects
            JSONArray jsonTableArray = jsonRootObject.optJSONArray("table");

            // Initialize the food and drink arrays based on length
            String tables[][];
            tables = new String[jsonTableArray.length()][3];
            Log.d("DEBUG:", "Table array items: " + jsonTableArray.length());

            //Iterate the jsonArray and populated the tables in the jsonTableArray
            for (int i = 0; i < jsonTableArray.length(); i++) {
                JSONObject jsonObject = jsonTableArray.getJSONObject(i);
                tables[i][0] = jsonObject.optString("tableNumber");
                tables[i][1] = jsonObject.optString("openStatus");
                tables[i][2] = jsonObject.optString("currentOrderNumber");
            }

            tableStatus = tables[tableNumber - 1][1];
            orderNumber = tables[tableNumber - 1][2];

            if (tableStatus.equals("1")) {
                Log.d("DEBUG: ", "Table " + tableNumber + " is already open.  Loading order.");
                orderNumberText.setText(orderNumber);
                loadOrder(orderNumber);
            }
            else{
                Log.d("DEBUG: ", "Table " + tableNumber + " is closed.  Opening table.");
                openTable(tableNumber);
            }

        } catch (JSONException e) {
            e.printStackTrace();
        }


    }

    public void openTable(final int tableNumber){
        Log.d("DEBUG: ", "Opening table " + tableNumber);
        RequestQueue requestQueue = Volley.newRequestQueue(this);
        String URL = serverURL + "/tableOperation.php";
        Log.d("DEBUG: ", "Opening table " + tableNumber + " with " + URL);

        StringRequest stringRequest = new StringRequest(Request.Method.POST, URL,
                new Response.Listener<String>() {
                    @Override
                    public void onResponse(String response) {
                        Log.i("VOLLEY", response);
                        Log.d("DEBUG: ", "table " + tableNumber + "has been opened.");
                        checkIfTableIsOpen(serverURL);
                    }
                },
                new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error) {
                        Log.e("VOLLEY", error.toString());
                    }
                }) {
            @Override
            protected Map<String, String> getParams() {
                Map<String, String> params = new HashMap<String, String>();

                params.put("operation", "1");
                params.put("tableNumber", Integer.toString(tableNumber));

                return params;
            }
        };
        requestQueue.add(stringRequest);
    }

    public void loadOrder(final String orderNumber){
        Log.d("DEBUG: ", "Loading order for " + orderNumber);
        RequestQueue requestQueue = Volley.newRequestQueue(this);
        String URL = serverURL + "/orderlist.php";
        Log.d("DEBUG: ", "Retrieving order #" + orderNumber + " from " + URL);

        StringRequest stringRequest = new StringRequest(Request.Method.POST, URL,
                new Response.Listener<String>() {
                    @Override
                    public void onResponse(String response) {
                        Log.i("VOLLEY", response);
                        updateOrderList(response);
                    }
                },
                new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error) {
                        Log.e("VOLLEY", error.toString());
                    }
                }) {
            @Override
            protected Map<String, String> getParams() {
                Map<String, String> params = new HashMap<String, String>();
                params.put("orderNumber", orderNumber);
                return params;
            }
        };
        requestQueue.add(stringRequest);
    }

    public void updateOrderList(String JSONstring){
        try {
            // Create the root JSONObject from the JSON string.
            JSONObject jsonRootObject = new JSONObject(JSONstring).getJSONObject("object");

            //Get the instance of JSONArray that contains JSONObjects
            JSONArray orderItemArray = jsonRootObject.optJSONArray("order");

            // Initialize the food and drink arrays based on length
            String orderItems[][];
            orderItems = new String[orderItemArray.length()][4];
            Log.d("DEBUG:", "orderItems array items: " + orderItemArray.length());

            //Iterate the jsonArray and populated the order item in the orderItemArray
            for (int i = 0; i < orderItemArray.length(); i++) {
                JSONObject jsonObject = orderItemArray.getJSONObject(i);
                orderItems[i][0] = jsonObject.optString("Item");
                orderItems[i][1] = jsonObject.optString("Quantity");
                orderItems[i][2] = jsonObject.optString("SubmitTime");
                orderItems[i][3] = jsonObject.optString("price");
            }

            // clear the textview in case you switch between order numbers
            summaryTextView.setText("");
            for (int i = 0; i < orderItems.length; i++){
                summaryTextView.append("\n" + "Table: " + tableNumber +
                        " --> " + orderItems[i][0] + " - $" + orderItems[i][3]);
            }


        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public boolean isTableValid(int x){
        if(x > 0 && x < 11)
            return true;
        else{
            Toast.makeText(getApplicationContext(),
                    "Invalid Table Number", Toast.LENGTH_SHORT).show();
            summaryTextView.setText("");
            return false;
        }
    }
}
