#import <UIKit/UIKit.h>
#import "UnityAppController.h"
#import "UI/UnityView.h"
#import "UI/UnityViewControllerBase.h"
#import "Branch.h"

@interface BranchAppController : UnityAppController
{
}
@end

@implementation BranchAppController

- (BOOL)application:(UIApplication *)application continueUserActivity:(NSUserActivity *)userActivity restorationHandler:(void (^)(NSArray *))restorationHandler {

    [super application:application continueUserActivity:userActivity restorationHandler:restorationHandler];

    BOOL handledByBranch = [[Branch getInstance] continueUserActivity:userActivity];
    
    NSLog(@"Branch continueUserActivity");
    
    return handledByBranch;
}

@end

IMPL_APP_CONTROLLER_SUBCLASS(BranchAppController)

